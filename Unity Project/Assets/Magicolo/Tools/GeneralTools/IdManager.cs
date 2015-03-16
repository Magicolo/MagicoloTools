using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;

namespace Magicolo.GeneralTools {
	[System.Serializable]
	public class IdManager<T> where T : IIdentifiable {

		Dictionary<int, T> idIdentifiableDict = new Dictionary<int, T>();
		Dictionary<int, T> IdIdentifiableDict {
			get {
				idIdentifiableDict = idIdentifiableDict ?? new Dictionary<int, T>();
				return idIdentifiableDict;
			}
		}

		int idCounter;
		
		public virtual T GetIdentifiableWithId(int id) {
			return IdIdentifiableDict[id];
		}
		
		public virtual int GetUniqueId() {
			idCounter += 1;
			return idCounter;
		}
		
		public virtual void SetUniqueId(T identifiable) {
			idCounter += 1;
			identifiable.Id = idCounter;
			IdIdentifiableDict[idCounter] = identifiable;
		}
		
		public virtual void SetUniqueIds(IList<T> identifiables) {
			foreach (T identifiable in identifiables) {
				SetUniqueId(identifiable);
			}
		}
		
		public virtual void ResetUniqueIds(IList<T> identifiables) {
			RemoveAllIds();
			SetUniqueIds(identifiables);
		}
		
		public virtual void AddId(int id, T identifiable) {
			IdIdentifiableDict[id] = identifiable;
		}
		
		public virtual void RemoveId(int id) {
			IdIdentifiableDict.Remove(id);
		}
		
		public virtual void RemoveAllIds() {
			IdIdentifiableDict.Clear();
			idCounter = 0;
		}
		
		public virtual bool ContainsId(int id){
			return IdIdentifiableDict.ContainsKey(id);
		}
		
		public virtual bool ContainsIdentifiable(T identifiable){
			return IdIdentifiableDict.ContainsValue(identifiable);
		}
	}
}

