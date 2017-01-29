// Copyright 2015-2025 Alessio Parma <alessio.parma@gmail.com>
//
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except
// in compliance with the License. You may obtain a copy of the License at:
//
// "http://www.apache.org/licenses/LICENSE-2.0"
//
// Unless required by applicable law or agreed to in writing, software distributed under the License
// is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express
// or implied. See the License for the specific language governing permissions and limitations under
// the License.

namespace PommaLabs.CodeServices.Common.Core
{
    /// <summary>
    ///   Error messages.
    /// </summary>
    internal static class ErrorMessages
    {
        public const string ContainedItem = "TODO";
        public const string EmptyList = "List is empty";
        public const string EmptyQueue = "Queue is empty";
        public const string EmptyStack = "Stack is empty";
        public const string NotContainedItem = "TODO";
        public const string NullList = "TODO";

        public const string TopLevel_Tuples_NotFullCollection = "Tuple is not (and cannot be) a full featured collection, therefore some IList methods are not implemented.";
        public const string TopLevel_Tuples_SequenceTooLarge = "Tuple can contain a fixed number of items, but given sequence is too large.";

        public const string Collections_MultiValueDictionary_KeyNotFound = "Given key has not been found inside the dictionary.";
        public const string Collections_MultiValueDictionary_ReadOnlyCollection = "Collection is readonly, thus it cannot be modified.";
        public const string Collections_MultiValueDictionary_Enumerator_BeforeCurrent = "Cursor is set before the beginning of the collection.";
        public const string Collections_MultiValueDictionary_Enumerator_AfterCurrent = "Cursor is set after the end of the collection.";
        public const string Collections_MultiValueDictionary_Enumerator_Modification = "Cannot enumerate a collection while it is being modified.";
        public const string Collections_MultiValueDictionary_Create_TValueCollectionReadOnly = "Given collection type is readonly.";
        public const string Collections_MultiValueDictionary_CopyTo_ArgumentsTooSmall = "Starting from given index, given array does not have enough space.";

        public const string Collections_ReadOnlyList_Immutable = "List is immutable and it cannot be modified.";
        public const string Collections_ReadOnlyList_NullItems = "Given items cannot be null, or Nothing in VB.NET.";

        public const string Reflection_ServiceLocator_ErrorOnLoading = "An error occurred while loading specified type. See inner exception for more details.";
        public const string Reflection_ServiceLocator_InterfaceNotImplemented = "Specified type does not implement given interface.";
        public const string Reflection_ServiceLocator_NotAnInterface = "Specified type argument is not an interface.";

        public const string Threading_ConcurrentWorkQueue_NullAction = "Given action cannot be null, or Nothing in VB.NET.";
    }
}