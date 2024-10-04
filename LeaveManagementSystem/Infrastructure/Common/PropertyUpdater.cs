namespace LeaveManagementSystem.Infrastructure.Common
{
    public static class PropertyUpdater
    {
        public static T UpdateIfChanged<T>(T existingValue, T newValue, ref bool isUpdated)
        {
            if (!EqualityComparer<T>.Default.Equals(existingValue, newValue))
            {
                isUpdated = true;
                return newValue;
            }
            return existingValue;
        }
    }
}
