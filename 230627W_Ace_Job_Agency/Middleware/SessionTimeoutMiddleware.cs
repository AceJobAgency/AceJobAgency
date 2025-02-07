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
                if (DateTime.Now - sessionStartTime > TimeSpan.FromSeconds(30)) {
                    context.Session.Clear();
                    context.Response.Redirect("/Login");
                    return;
                }
            }

            await _next(context);
        }
    }
}
