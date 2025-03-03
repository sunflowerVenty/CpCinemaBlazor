using CpCinemaBlazor.ApiRequest.Model;
using CpCinemaBlazor.ApiRequest.Services;

namespace CpCinemaBlazor.ApiRequest
{
    public static class SingletoneUser
    {
        public static CurUser us;

        public static void InitUser(CurUser user)
        {
            us = user;
        }

        public static CurUser GetUser()
        {
            return us;
        }

        public static void exitUser()
        {
            us = null;
        }

    }
}
