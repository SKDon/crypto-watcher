﻿using CryptoWatcher.Application.Users.Requests;


namespace CryptoWatcher.Application.Users.FakeRequests
{
    public static class AddUserFakeRequest
    {
        public static AddUserRequest GetFake_1()
        {
            return new AddUserRequest
            {
                UserId = "johny.melavo"
            };
        }       
    }
}