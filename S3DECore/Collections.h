#pragma once
#include "EngineTypes.h"

namespace S3DECore {
	namespace Collections {
		
		generic <typename T>
		public ref class LinkedQueueList {
		public:
			LinkedQueueList();
			uint Count();
			T Dequeue();
			void Enqueue(T);
			bool TryRemove(T);
		private:
			ref class Entry {
			public:
				Entry(T val);
				T GetValue();
				Entry^ GetParent();
				Entry^ GetChild();
				void SetChild(Entry^ e);
				void SetParent(Entry^ e);
			private:
				T value;
				Entry ^parent, ^child;
			};
			uint length;
			Entry ^head, ^tail;
		};
		
	}
}