// Set default scheduled publish datetime to current time
document.addEventListener('DOMContentLoaded', function() {
    const scheduledInput = document.getElementById('scheduled-publish-input');
    
    if (scheduledInput && !scheduledInput.value) {
        // Get current datetime in local timezone
        const now = new Date();
        
        // Format to datetime-local format: YYYY-MM-DDTHH:MM
        const year = now.getFullYear();
        const month = String(now.getMonth() + 1).padStart(2, '0');
        const day = String(now.getDate()).padStart(2, '0');
        const hours = String(now.getHours()).padStart(2, '0');
        const minutes = String(now.getMinutes()).padStart(2, '0');
        
        const formattedDateTime = `${year}-${month}-${day}T${hours}:${minutes}`;
        
        scheduledInput.value = formattedDateTime;
        console.log('? Set default scheduled publish time:', formattedDateTime);
    }
});
