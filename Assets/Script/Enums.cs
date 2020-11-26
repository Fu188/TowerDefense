using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts
{
    public enum PeopleState
    {
        Inactive,
        Searching,
        Targeting
    }

    public enum GameState
    {
        Start,
        Playing,
        Won,
        Lost
    }
}