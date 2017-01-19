using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Collections;

namespace gitdb.Entities
{
	public class EntityLinker
	{
		public EntityLinker ()
		{
		}

		public void AddLink(BaseEntity entity, string propertyName, BaseEntity linkedEntity)
		{
			var otherPropertyName = "";

			var property = entity.GetType ().GetProperty (propertyName);

			if (PropertyHasLinkAttribute (property, out otherPropertyName)
				|| IsLinkProperty(entity, property)) {
				var value = property.GetValue (entity);
				var newValue = AddEntityToObject (linkedEntity, value, property);
				property.SetValue (entity, newValue);

				entity.IsPendingLinkCommit = true;

				if (!String.IsNullOrEmpty(otherPropertyName))
					AddReturnLink (entity, property, linkedEntity, otherPropertyName);
			} else
				throw new Exception ("Invalid property. It needs to have the type BaseEntity.");
		}

		public void RemoveReturnLink(BaseEntity entity, PropertyInfo property, BaseEntity targetEntity, string targetEntityPropertyName)
		{
			if (entity == null)
				throw new ArgumentNullException ("entity");
			
			if (property == null)
				throw new ArgumentNullException ("property");
			
			if (targetEntity == null)
				throw new ArgumentNullException ("targetEntity", "Entity '" + entity.GetType().FullName + "', property '" + property.Name + "'.");

			var targetEntityProperty = targetEntity.GetType ().GetProperty (targetEntityPropertyName);

			if (targetEntityProperty == null)
				throw new Exception ("Property '" + targetEntityPropertyName + "' not found on type '" + targetEntity.GetType().FullName + "'.");
			else {
				var existingReturnLinksObject = targetEntityProperty.GetValue (targetEntity);

				var newReturnLinksObject = RemoveEntityFromObject (entity, existingReturnLinksObject, targetEntityProperty);

				targetEntityProperty.SetValue (targetEntity, newReturnLinksObject);
			}
		}

		public bool PropertyHasLinkAttribute(PropertyInfo property, out string otherPropertyName)
		{
			var attributes = property.GetCustomAttributes ();

			foreach (var attribute in attributes) {
				if (attribute is OneWayAttribute) {
					otherPropertyName = "";
					return true;
				}
				else if (attribute is TwoWayAttribute) {
					otherPropertyName = ((TwoWayAttribute)attribute).OtherPropertyName;
					return true;
				}
			}

			otherPropertyName = "";

			return false;
		}

		public string GetOtherPropertyName(PropertyInfo property)
		{
			var otherPropertyName = String.Empty;

			var attributes = property.GetCustomAttributes ();

			foreach (var attribute in attributes) {
				if (attribute is OneWayAttribute) {
					otherPropertyName = "";
				}
				else if (attribute is TwoWayAttribute) {
					otherPropertyName = ((TwoWayAttribute)attribute).OtherPropertyName;
				}
			}

			return otherPropertyName;
		}

		public bool IsLinkProperty(BaseEntity entity, PropertyInfo property)
		{
			return IsEntityProperty (property)
				|| IsEntityArrayProperty (property);
		}

		public bool IsEntityProperty(PropertyInfo property)
		{
			var isEntity = property.PropertyType.IsSubclassOf (typeof(BaseEntity));

			return isEntity;
		}

		public bool IsEntityArrayProperty(PropertyInfo property)
		{
			var isEntityArray = property.PropertyType.IsArray
				&& property.PropertyType.GetElementType ().IsSubclassOf (typeof(BaseEntity));

			return isEntityArray;
		}

		public void AddReturnLink(BaseEntity entity, PropertyInfo property, BaseEntity targetEntity, string targetEntityPropertyName)
		{
			var targetEntityType = targetEntity.GetType ();

			var targetEntityProperty = targetEntityType.GetProperty (targetEntityPropertyName);

			if (targetEntityProperty == null)
				throw new ArgumentException ("The property '" + targetEntityPropertyName + "' was not found on the '" + targetEntity.GetType ().Name + "' entity.");
			var existingReturnLinksObject = targetEntityProperty.GetValue (targetEntity);

			var newReturnLinksObject = AddEntityToObject (entity, existingReturnLinksObject, targetEntityProperty);

		    targetEntityProperty.SetValue (targetEntity, newReturnLinksObject);

			entity.IsPendingLinkCommit = true;
		}

		public object AddEntityToObject(BaseEntity entityToAdd, object linksObject, PropertyInfo property)
		{
			if (IsEntityProperty(property)) {
				return entityToAdd;
			}
			else if (IsEntityArrayProperty(property)) {

				var list = new ArrayList ();

				if (linksObject != null)
					list.AddRange ((BaseEntity[])linksObject);
				
				// TODO: Should this check that it's not already in the list? Check via id, not via object comparison
				if (!EntityExists((BaseEntity[])list.ToArray(typeof(BaseEntity)), entityToAdd))
					list.Add (entityToAdd);

			    return list.ToArray (entityToAdd.GetType ());
			} else
				throw new Exception ("Invalid return link object. Must be subclass of BaseEntity or BaseEntity[] array.");            
		}

		public object RemoveEntityFromObject(BaseEntity entityToRemove, object linksObject, PropertyInfo property)
		{
			if (IsEntityProperty(property)) {
				return null;
			}
			else if (IsEntityArrayProperty(property)) {
				var list = new ArrayList ();

				if (linksObject != null)
					list.AddRange ((BaseEntity[])linksObject);

				for (int i = 0; i < list.Count; i++) {
					if (((BaseEntity)list [i]).Id == entityToRemove.Id) {
						list.RemoveAt (i);
						break;
					}
				}

				return list.ToArray (entityToRemove.GetType());
			} else
				throw new Exception ("Invalid return link object. Must be subclass of BaseEntity or BaseEntity[] array.");

		}

		public BaseEntity[] GetLinkedEntities (BaseEntity entity, PropertyInfo property)
		{
			var list = new List<BaseEntity> ();

			var value = property.GetValue (entity);

			if (value != null) {
				if (IsEntityProperty (property)) {
					list.Add ((BaseEntity)value);
				} else {
					if (((BaseEntity[])value).Length > 0)
						list.AddRange ((BaseEntity[])value);
				}
			}

			return list.ToArray ();
		}

		public bool EntityExists(BaseEntity[] entities, BaseEntity entityToFind)
		{
			if (entities == null)
				return false;
			
			if (entityToFind == null)
				throw new ArgumentNullException ("entityToFind");
			
			return (from entity in entities
				where entity != null
					&& entity.GetType() == entityToFind.GetType()
			        && entity.Id == entityToFind.Id
				select entity).Count() > 0;
		}
	}
}

