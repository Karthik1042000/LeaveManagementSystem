namespace LeaveManagementSystem.Infrastructure.Common
{
    public static class PropertyUpdater
    {
        public static T UpdateIfChanged<T>(T existingValue, T newValue, ref bool isUpdated)
        {
            // If the values are not equal, return the new value and set isUpdated to true
            if (!EqualityComparer<T>.Default.Equals(existingValue, newValue))
            {
                isUpdated = true;
                return newValue;
            }
            // If the values are equal, return the existing value
            return existingValue;
        }
    }
}
