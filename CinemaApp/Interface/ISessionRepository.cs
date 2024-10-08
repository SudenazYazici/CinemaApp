﻿using CinemaApp.Models;

namespace CinemaApp.Interface
{
    public interface ISessionRepository
    {
        ICollection<Session> GetSessions();
        Session GetSession(int id);
        Session GetSession(int cinemaId, int movieId, int cinemaHallId, DateTime startTime);
        ICollection<Session> GetSessions(int cinemaId, int movieId, int cinemaHallId);
        //Session GetSession(int movieId, int cinemaHallId, DateTime startTime);

        bool SessionExists(int id);
        bool CreateSession(Session session);
        bool UpdateSession(Session session);
        bool DeleteSession(Session session);
        bool Save();
        ICollection<Session> GetSessionsOfMovie(int movieId);
        ICollection<CinemaHall> GetCinemaHallsOfMovie(int movieId);
        //ICollection<DateTime> GetSessionsStartTime(int movieId, int cinemaHallId);
    }
}
