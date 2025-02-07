namespace _230627W_Ace_Job_Agency.Middleware {
    public class SessionTimeoutMiddleware {
        private readonly RequestDelegate _next;

        public SessionTimeoutMiddleware(RequestDelegate next) {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context) {
            var sessionStartTimeVar = context.Session.GetString("SessionStartTime");
            if (context.Session.IsAvailable && sessionStartTimeVar != null) {
                DateTime sessionStartTime = DateTime.Parse(sessionStartTimeVar);
                // Checking if session has expired (greater than 30 seconds)
                if (DateTime.Now - sessionStartTime > TimeSpan.FromSeconds(30)) {
                    context.Session.Clear(); // Clears the session
                    context.Response.Redirect("/Login"); // Redirect to login page
                    return;
                }
            }

            // Proceed to the next middleware
            await _next(context);
        }
    }
}
