using JumpKick.HttpLib.Builder;
using System;

namespace JumpKick.HttpLib
{
    public static class Http
    {
        public static RequestBuilder Get(string url)
        {
            return new RequestBuilder(url, HttpVerb.Get);
        }

        public static RequestBuilder Head(string url)
        {
            return new RequestBuilder(url, HttpVerb.Head);
        }

        public static RequestBuilder Post(string url)
        {
            return new RequestBuilder(url, HttpVerb.Post);
        }

        public static RequestBuilder Put(string url)
        {
            return new RequestBuilder(url, HttpVerb.Put);
        }

        public static RequestBuilder Patch(string url)
        {
            return new RequestBuilder(url, HttpVerb.Patch);
        }

        public static RequestBuilder Delete(string url)
        {
            return new RequestBuilder(url, HttpVerb.Delete);
        }
    }
}
