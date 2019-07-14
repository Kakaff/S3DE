#include "Collections.h"

namespace S3DECore {
	namespace Collections {
		
		generic <typename T>
			LinkedQueueList<T>::LinkedQueueList() {}

		generic <typename T>
			uint LinkedQueueList<T>::Count() { return length; }

		generic <typename T>
			T LinkedQueueList<T>::Dequeue() {

			Entry ^h = head;

			if (h != nullptr) {
				if (head->GetChild() != nullptr) {
					head = head->GetChild();
					head->SetParent(nullptr);
				}
				else {
					head = nullptr;
					tail = nullptr;
				}
				length--;
			}
			if (h != nullptr)
				return h->GetValue();
			else
				return T();
		}

		generic <typename T>
			bool LinkedQueueList<T>::TryRemove(T val) {
				if (length == 0)
					return false;

				Entry^ e = head;
				while (e->GetValue()->GetHashCode() != val->GetHashCode()) {
					if (e->GetChild() == nullptr)
						return false;
					else
						e = e->GetChild();
				}

				if (e->GetParent() != nullptr) {
					e->GetParent()->SetChild(e->GetChild());
					if (e->GetChild() != nullptr)
						e->GetChild()->SetParent(e->GetParent());
					else
						tail = e->GetParent();
				}
				else {
					head = e->GetChild();
					if (head != nullptr)
						head->SetParent(nullptr);
				}
				length--;
				return true;
		}

		generic <typename T>
			void LinkedQueueList<T>::Enqueue(T val) {
				Entry^ newTail = gcnew Entry(val);
				newTail->SetParent(tail);
				if (tail != nullptr)
					tail->SetChild(newTail);
				if (head == nullptr)
					head = newTail;

				tail = newTail;
				length++;
			}


		generic <typename T>
			LinkedQueueList<T>::Entry::Entry(T val) { value = val; }

		generic <typename T>
			T LinkedQueueList<T>::Entry::GetValue() { return value; }

		generic <typename T>
			LinkedQueueList<T>::Entry^ LinkedQueueList<T>::Entry::GetParent() { return parent; }

		generic <typename T>
			LinkedQueueList<T>::Entry^ LinkedQueueList<T>::Entry::GetChild() { return child; }

		generic <typename T>
			void LinkedQueueList<T>::Entry::SetParent(Entry^ e) { parent = e; }

		generic <typename T>
			void LinkedQueueList<T>::Entry::SetChild(Entry^ e) { child = e; }

			
	}
}