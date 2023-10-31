using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace LetMeDieCSharp3
{
    class UserContext : DbContext
    {
        public UserContext()
        : base("DbConnection")
        { }
        public DbSet<Cinema> Cinemas { get; set; }
        public DbSet<Hall> Halls { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Genre> Genres { get; set; }

    }
    public class Cinema
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Hall> Halls { get; set; }
    }
    public class Hall
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public int? CinemaId { get; set; }
        public Cinema Cinema { get; set; }
        public ICollection<Movie> Movies { get; set; }

        public Hall()
        {
            Movies = new List<Movie>();
        }
    }

    public class Movie
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Rating { get; set; }
        public int? GenreId { get; set; }
        public Genre Genre { get; set; }

        public ICollection<Hall> Halls { get; set; }

        public Movie()
        {
            Halls = new List<Hall>();
        }
    }

    public class Genre
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Movie> Movies { get; set; }

    }




    class Program
    {
        static void Main(string[] args)
        {

            using (UserContext db = new UserContext())
            {
                void printMovies()
                {
                    Console.WriteLine("Name Genre Rating ");
                    foreach (Movie m in db.Movies.Include(p => p.Genre))
                    {
                        Console.WriteLine("{0} {1} {2}", m.Name, m.Genre != null ? m.Genre.Name : "", m.Rating);
                    }
                }
                void printCinemas()
                {
                    Console.WriteLine("Name Halls ");
                    foreach (Cinema pl in db.Cinemas.Include(p => p.Halls))
                    {
                        Console.WriteLine("{0}", pl.Name);
                        foreach (Hall m in pl.Halls)
                        {
                            Console.WriteLine("-{0}", m.Number);

                        }
                    }
                }
                void printHalls()
                {
                    Console.WriteLine("Hall_Number Movies ");
                    foreach (Hall pl in db.Halls.Include(p => p.Movies))
                    {
                        Console.WriteLine("{0} | {1}",pl.Id, pl.Number);
                        //string movieS = 
                        foreach (Movie m in pl.Movies)
                        {
                            Console.WriteLine("-{0}", m.Name);

                        }
                        //Console.WriteLine();
                    }

                }
                void printGenres()
                {
                    Console.WriteLine("Name Films ");
                    foreach (Genre pl in db.Genres.Include(p => p.Movies))
                    {
                        Console.WriteLine("{0}", pl.Name);
                        foreach (Movie m in pl.Movies)
                        {
                            Console.WriteLine("-{0}", m.Name);

                        }
                    }
                }

                void changeGenres()
                {

                    int k = -1;
                    string genrename = "";
                    Console.WriteLine("Введите название жанра, который вы хотите изменить:");
                    genrename = Console.ReadLine();
                    if(genrename!="")
                    {
                        Genre genre = db.Genres.Where(x => x.Name == genrename).FirstOrDefault();
                        do
                        {

                            
                            Console.WriteLine("1 - Изменить название");
                            Console.WriteLine("0 - Выход");
                            Console.WriteLine("Выберите вариант:");
                            k = Convert.ToInt32(Console.ReadLine());
                            switch (k)
                            {

                                case 1:
                                    {
                                        string newgenrename = "";
                                        Console.WriteLine("Введите новое название жанра:");
                                        newgenrename = Console.ReadLine();
                                        
                                        genre.Name = newgenrename; // изменим название
                                        db.Entry(genre).State = EntityState.Modified;
                                        db.SaveChanges();

                                        break;
                                    }
                                case 0:
                                    {

                                        break;
                                    }

                            }

                        } while (k != 0);
                    }
                    
                }
                void changeCinemas()
                {

                    int k = -1;
                    string cinemaname = "";
                    printCinemas();
                    Console.WriteLine("Введите название кинотеатра, который вы хотите изменить:");
                    cinemaname = Console.ReadLine();
                    if (cinemaname != "")
                    {
                        Cinema cinema = db.Cinemas.Where(x => x.Name == cinemaname).FirstOrDefault();
                        do
                        {


                            Console.WriteLine("1 - Изменить название");
                            Console.WriteLine("2 - Добавить кинозал");
                            Console.WriteLine("3 - Удалить кинозал");
                            Console.WriteLine("0 - Выход");
                            Console.WriteLine("Выберите вариант:");
                            k = Convert.ToInt32(Console.ReadLine());
                            switch (k)
                            {

                                case 1:
                                    {
                                        string newcinemaname = "";
                                        Console.WriteLine("Введите новое название кинотеатра:");
                                        newcinemaname = Console.ReadLine();

                                        cinema.Name = newcinemaname; // изменим название
                                        db.Entry(cinema).State = EntityState.Modified;
                                        db.SaveChanges();

                                        break;
                                    }
                                case 2:
                                    {
                                        printHalls();
                                        int hallId = 0;
                                        Console.WriteLine("Введите индекс кинозала, который нужно вставить:");
                                        hallId = Convert.ToInt32(Console.ReadLine());

                                        Hall hall = db.Halls.Where(x => x.Id == hallId).FirstOrDefault();
                                        if(hall.CinemaId == null)
                                        {
                                            cinema.Halls.Add(hall);
                                            db.Entry(cinema).State = EntityState.Modified;
                                            db.SaveChanges();
                                        }
                                        

                                        break;
                                    }
                                case 3:
                                    {
                                        printHalls();
                                        int hallId = 0;
                                        Console.WriteLine("Введите индекс кинозала, который нужно удалить:");
                                        hallId = Convert.ToInt32(Console.ReadLine());

                                        Hall hall = db.Halls.Where(x => x.Id == hallId).FirstOrDefault();
                                        if (hall.CinemaId == cinema.Id)
                                        {
                                            cinema.Halls.Remove(hall);
                                            db.Entry(cinema).State = EntityState.Modified;
                                            db.SaveChanges();
                                        }


                                        break;
                                    }
                                case 0:
                                    {

                                        break;
                                    }

                            }

                        } while (k != 0);
                    }

                }

                void changeMovies()
                {

                    int k = -1;
                    string moviename = "";
                    printMovies();
                    Console.WriteLine("Введите название фильма, который вы хотите изменить:");
                    moviename = Console.ReadLine();
                    if (moviename != "")
                    {
                        Movie movie = db.Movies.Where(x => x.Name == moviename).FirstOrDefault();
                        do
                        {


                            Console.WriteLine("1 - Изменить название");
                            Console.WriteLine("2 - Изменить жанр");
                            Console.WriteLine("3 - Изменить рейтинг");
                            Console.WriteLine("4 - Добавить кинозал на показ фильма");
                            Console.WriteLine("5 - Убрать кинозал с показа фильма");
                            Console.WriteLine("0 - Выход");
                            Console.WriteLine("Выберите вариант:");
                            k = Convert.ToInt32(Console.ReadLine());
                            switch (k)
                            {

                                case 1:
                                    {
                                        string newmoviename = "";
                                        Console.WriteLine("Введите новое название кинотеатра:");
                                        newmoviename = Console.ReadLine();

                                        movie.Name = newmoviename; // изменим название
                                        db.Entry(movie).State = EntityState.Modified;
                                        db.SaveChanges();

                                        break;
                                    }
                                case 2:
                                    {
                                        printGenres();
                                        string newgenrename = "";
                                        Console.WriteLine("Введите новый жанра кинотеатра:");
                                        newgenrename = Console.ReadLine();

                                        Genre genre = db.Genres.Where(x => x.Name == newgenrename).FirstOrDefault();

                                        movie.Genre = genre;
                                        db.Entry(movie).State = EntityState.Modified;
                                        db.SaveChanges();


                                        break;
                                    }
                                case 3:
                                    {
                                        double movieRate = 0;
                                        Console.WriteLine("Введите новый рейтинг фильма:");
                                        movieRate = Convert.ToDouble(Console.ReadLine());

                                        if (movieRate != 0)
                                        {
                                            movie.Rating = movieRate; // изменим название
                                            db.Entry(movie).State = EntityState.Modified;
                                            db.SaveChanges();
                                        }

                                        break;
                                    }
                                case 4:
                                    {
                                        printHalls();
                                        int hallId = 0;
                                        Console.WriteLine("Введите индекс кинозала, который нужно вставить:");
                                        hallId = Convert.ToInt32(Console.ReadLine());

                                        Hall hall = db.Halls.Where(x => x.Id == hallId).FirstOrDefault();
                                        if (!movie.Halls.Contains(hall))
                                        {
                                            movie.Halls.Add(hall);
                                            db.Entry(movie).State = EntityState.Modified;
                                            db.SaveChanges();
                                        }


                                        break;
                                    }
                                case 5:
                                    {
                                        printHalls();
                                        int hallId = 0;
                                        Console.WriteLine("Введите индекс кинозала, который нужно удалить:");
                                        hallId = Convert.ToInt32(Console.ReadLine());

                                        Hall hall = db.Halls.Where(x => x.Id == hallId).FirstOrDefault();
                                        if (movie.Halls.Contains(hall))
                                        {
                                            movie.Halls.Remove(hall);
                                            db.Entry(movie).State = EntityState.Modified;
                                            db.SaveChanges();
                                        }


                                        break;
                                    }
                                case 0:
                                    {

                                        break;
                                    }

                            }

                        } while (k != 0);
                    }

                }
                void changeHalls()
                {

                    int k = -1;
                    int hallid = -1;
                    printHalls();
                    Console.WriteLine("Введите индекс зала, который вы хотите изменить:");
                    hallid = Convert.ToInt32(Console.ReadLine());
                    if (hallid != -1)
                    {
                        Hall hall = db.Halls.Where(x => x.Id == hallid).FirstOrDefault();
                        do
                        {


                            Console.WriteLine("1 - Изменить номер");
                            Console.WriteLine("2 - Добавить показ фильма в зал");
                            Console.WriteLine("3 - Убрать показ фильма из зала");
                            Console.WriteLine("0 - Выход");
                            Console.WriteLine("Выберите вариант:");
                            k = Convert.ToInt32(Console.ReadLine());
                            switch (k)
                            {

                                case 1:
                                    {
                                        int newhallnum = -1;
                                        Console.WriteLine("Введите новый номер зала:");
                                        newhallnum = Convert.ToInt32(Console.ReadLine());
                                        if(newhallnum!=-1)
                                        {
                                            hall.Number = newhallnum; // изменим название
                                            db.Entry(hall).State = EntityState.Modified;
                                            db.SaveChanges();
                                        }
                                        

                                        break;
                                    }
                                case 2:
                                    {
                                        printMovies();
                                        string moviename = "";
                                        Console.WriteLine("Введите название фильма, который нужно вставить:");
                                        moviename = Console.ReadLine();

                                        Movie movie = db.Movies.Where(x => x.Name== moviename).FirstOrDefault();
                                        if (!hall.Movies.Contains(movie))
                                        {
                                            hall.Movies.Add(movie);
                                            db.Entry(hall).State = EntityState.Modified;
                                            db.SaveChanges();
                                        }


                                        break;
                                    }
                                case 3:
                                    {
                                        printMovies();
                                        string moviename = "";
                                        Console.WriteLine("Введите название фильма, который нужно убрать из зала:");
                                        moviename = Console.ReadLine();

                                        Movie movie = db.Movies.Where(x => x.Name == moviename).FirstOrDefault();
                                        if (hall.Movies.Contains(movie))
                                        {
                                            hall.Movies.Remove(movie);
                                            db.Entry(hall).State = EntityState.Modified;
                                            db.SaveChanges();
                                        }


                                        break;
                                    }
                                case 0:
                                    {

                                        break;
                                    }

                            }

                        } while (k != 0);
                    }

                }

                void removeGenre()
                {
                    printGenres();
                    int k = -1;
                    string genrename = "";
                    Console.WriteLine("Введите название жанра, который вы хотите удалить:");
                    genrename = Console.ReadLine();
                    if (genrename != "")
                    {
                        Genre genre = db.Genres.Where(x => x.Name == genrename).FirstOrDefault();
                        db.Genres.Remove(genre);
                        db.SaveChanges();
                    }

                }
                void removeCinema()
                {
                    printCinemas();
                    int k = -1;
                    string cinemaname = "";
                    Console.WriteLine("Введите название кинотеатра, который вы хотите удалить:");
                    cinemaname = Console.ReadLine();

                    
                    Cinema cinema = db.Cinemas.Where(x => x.Name == cinemaname).FirstOrDefault();
                    int cinemaId = cinema.Id;
                    while(cinema.Halls.Count!=0)
                    {
                        Hall hall = cinema.Halls.First();
                        db.Halls.Remove(hall);
                        cinema.Halls.Remove(hall);
                        db.SaveChanges();
                    }

                    //foreach (Hall hall in cinema.Halls)
                    //{

                    //    hall.CinemaId = null;
                    //    hall.Movies.Clear();
                    //    hall.Cinema = null;
                    //    db.Halls.Remove(hall);
                    //    db.SaveChanges();



                    //}
                    foreach (Cinema pl in db.Cinemas.Include(p => p.Halls))
                    {
                        if (pl.Name == cinemaname)
                        {

                            pl.Halls.Clear();
                            //db.SaveChanges();

                        }
                    }
                    //while(cinema.Halls.)


                    //int cinemaId = -1;
                    if (cinemaname != "")
                    {

                        
                        
                        db.Cinemas.Remove(cinema);
                        db.SaveChanges();
                    }

                    


                }

                void removeMovie()
                {
                    printMovies();
                    int k = -1;
                    string moviename = "";
                    Console.WriteLine("Введите название фильма, который вы хотите удалить:");
                    moviename = Console.ReadLine();
                    if (moviename != "")
                    {
                        Movie movie = db.Movies.Where(x => x.Name == moviename).FirstOrDefault();
                        db.Movies.Remove(movie);
                        db.SaveChanges();
                    }

                }
                void removeHall()
                {
                    printHalls();
                    int k = -1;
                    int hallid = -1;
                    Console.WriteLine("Введите индекс зала, который вы хотите удалить:");
                    hallid = Convert.ToInt32(Console.ReadLine());
                    if (hallid != -1)
                    {
                        Hall hall = db.Halls.Where(x => x.Id == hallid).FirstOrDefault();
                        db.Halls.Remove(hall);
                        db.SaveChanges();
                    }

                }
                Genre genreHorror = new Genre { Name = "Horror" };
                Genre genreThriller = new Genre { Name = "Thriller" };
                Genre genreDrama = new Genre { Name = "Drama" };
                Genre genreMusical = new Genre { Name = "Musical" };
                Genre genreTragedy = new Genre { Name = "Tragedy" };
                Genre genreComedy = new Genre { Name = "Comedy" };
                List<Genre> genres =
                    new List<Genre> { genreHorror, genreThriller,genreDrama,genreMusical,genreTragedy,genreComedy};


                Cinema cinemaHorrorWorld= new Cinema { Name = "Horror World Cinema" };
                Cinema cinemaDramaQueen = new Cinema { Name = "Drama Queen Cinema" };
                Cinema cinemaGeneral = new Cinema { Name = "General Movie Theater" };
                List<Cinema> cinemas =
                    new List<Cinema> { cinemaHorrorWorld, cinemaDramaQueen, cinemaGeneral};

                int c = -1;
                do
                {
                    Console.WriteLine("Выберите вариант:");
                    Console.WriteLine("1 - Вывести таблицу");
                    Console.WriteLine("2 - Добавить значение в таблицу");
                    Console.WriteLine("3 - Изменить значение в таблице");
                    Console.WriteLine("4 - Удалить значение в таблице");
                    Console.WriteLine("0 - Выход");
                    c = Convert.ToInt32(Console.ReadLine());
                    switch (c)
                    {
                        case 1:
                        {
                                int r = -1;
                                do
                                {
                                    Console.WriteLine("Выберите вариант:");
                                    Console.WriteLine("1 - Жанры");
                                    Console.WriteLine("2 - Фильмы");
                                    Console.WriteLine("3 - Кинотеатры");
                                    Console.WriteLine("4 - Залы");
                                    Console.WriteLine("0 - Выход");
                                    r = Convert.ToInt32(Console.ReadLine());
                                    switch (r)
                                    {
                                        case 1:
                                            {

                                                printGenres();
                                                break;
                                            }
                                        case 2:
                                            {

                                                printMovies();
                                                break;
                                            }
                                        case 3:
                                            {

                                                printCinemas();
                                                break;
                                            }
                                        case 4:
                                            {
                                                printHalls();
                                                break;
                                            }
                                        default:
                                            {
                                                Console.WriteLine("Нет такого варианта");
                                                break;
                                            }
                                    }

                                } while (r!=0);
                                
                                break;
                        }
                        case 2:
                        {
                                int r = -1;
                                do
                                {
                                    Console.WriteLine("Выберите вариант:");
                                    Console.WriteLine("1 - Жанры");
                                    Console.WriteLine("2 - Фильмы");
                                    Console.WriteLine("3 - Кинотеатры");
                                    Console.WriteLine("4 - Залы");
                                    Console.WriteLine("0 - Выход");
                                    r = Convert.ToInt32(Console.ReadLine());
                                    switch (r)
                                    {
                                        case 1:
                                            {

                                                printGenres();
                                                string genreName = "";
                                                Console.WriteLine("Введите название нового жанра:");
                                                genreName = Console.ReadLine();
                                                Genre genre = new Genre { Name = genreName };
                                                db.Genres.Add(genre);
                                                db.SaveChanges();
                                                printGenres();
                                                break;
                                            }
                                        case 2:
                                            {

                                                printMovies();
                                                string movieName = "";
                                                string genreName = "";
                                                double rating = 0;
                                                Console.WriteLine("Введите название нового фильма:");
                                                movieName = Console.ReadLine();
                                                Console.WriteLine("Введите рейтинг нового фильма:");
                                                rating = Convert.ToDouble(Console.ReadLine());
                                                printGenres();
                                                Console.WriteLine("Введите жанр нового фильма:");
                                                genreName = Console.ReadLine();
                                                Movie movie =
                                                    new Movie
                                                    {
                                                        Name = movieName,
                                                        Rating = rating,
                                                        Genre = db.Genres.Where(p => p.Name == genreName ).FirstOrDefault()
                                                    };
                                                db.Movies.Add(movie);
                                                db.SaveChanges();
                                                printMovies();
                                                break;
                                            }
                                        case 3:
                                            {
                                                printCinemas();
                                                string cinemaName = "";
                                                Console.WriteLine("Введите название нового кинотеатра:");
                                                cinemaName = Console.ReadLine();
                                                Cinema cinema = new Cinema { Name = cinemaName };
                                                db.Cinemas.Add(cinema);
                                                db.SaveChanges();
                                                printCinemas();
                                                break;
                                            }
                                        case 4:
                                            {
                                                printHalls();
                                                int number = 0;
                                                string cinemaName = "";
                                                string movieName = "";
                                                Console.WriteLine("Введите номер нового зала:");
                                                number = Convert.ToInt32(Console.ReadLine());
                                                printCinemas();
                                                Console.WriteLine("Введите название кинотеатра, в котором находится зал:");
                                                cinemaName = Console.ReadLine();
                                                Hall hall =
                                                    new Hall
                                                    {
                                                        Number = number,
                                                        Cinema = db.Cinemas.Where(p => p.Name == cinemaName).FirstOrDefault()
                                                    };
                                                db.Halls.Add(hall);
                                                db.SaveChanges();
                                                printHalls();
                                                break;
                                            }
                                        case 0:
                                            {
                                                Console.WriteLine("Совершён выход");
                                                break;
                                            }
                                        default:
                                            {
                                                Console.WriteLine("Нет такого варианта");
                                                break;
                                            }
                                    }

                                } while (r != 0);

                                break;
                        }
                        case 3:
                        {
                                int r = -1;
                                do
                                {
                                    Console.WriteLine("Выберите вариант:");
                                    Console.WriteLine("1 - Жанры");
                                    Console.WriteLine("2 - Фильмы");
                                    Console.WriteLine("3 - Кинотеатры");
                                    Console.WriteLine("4 - Залы");
                                    Console.WriteLine("0 - Выход");
                                    r = Convert.ToInt32(Console.ReadLine());
                                    switch (r)
                                    {
                                        case 1:
                                            {

                                                changeGenres();
                                                printGenres();
                                                
                                                break;
                                            }
                                        case 2:
                                            {

                                                changeMovies();
                                                printMovies();
                                                
                                                break;
                                            }
                                        case 3:
                                            {
                                                changeCinemas();
                                                printCinemas();
                                               
                                                break;
                                            }
                                        case 4:
                                            {
                                                changeHalls();
                                                printHalls();
                                                break;
                                            }
                                        case 0:
                                            {
                                                Console.WriteLine("Совершён выход");
                                                break;
                                            }
                                        default:
                                            {
                                                Console.WriteLine("Нет такого варианта");
                                                break;
                                            }
                                    }

                                } while (r != 0);
                                break;
                        }
                        case 4:
                        {
                                int r = -1;
                                do
                                {
                                    Console.WriteLine("Выберите вариант:");
                                    Console.WriteLine("1 - Жанры");
                                    Console.WriteLine("2 - Фильмы");
                                    Console.WriteLine("3 - Кинотеатры");
                                    Console.WriteLine("4 - Залы");
                                    Console.WriteLine("0 - Выход");
                                    r = Convert.ToInt32(Console.ReadLine());
                                    switch (r)
                                    {
                                        case 1:
                                            {
                                                
                                                removeGenre();
                                                printGenres();
                                                break;
                                            }
                                        case 2:
                                            {


                                                removeMovie();
                                                printMovies();
                                                break;
                                            }
                                        case 3:
                                            {

                                                removeCinema();
                                                printCinemas();
                                                break;
                                            }
                                        case 4:
                                            {

                                                removeHall();
                                                printHalls();
                                                break;
                                            }
                                        case 0:
                                            {
                                                Console.WriteLine("Совершён выход");
                                                break;
                                            }
                                        default:
                                            {
                                                Console.WriteLine("Нет такого варианта");
                                                break;
                                            }
                                    }

                                } while (r != 0);
                                break;
                            }
                    
                    }

                } while (c != 0);
                


                 //db.Movies.Where(c => c.Name == "Titanic").FirstOrDefault().Halls.Add(db.Halls.Where(c => c.Number == 5).FirstOrDefault());
                 //db.SaveChanges();
                //db.Cinemas.AddRange(cinemas);
                //db.SaveChanges();
                //for (int i = 1; i <= 5; i++)
                //{
                //    Movie g = db.Movies.Where(c => c.Id == i).FirstOrDefault();
                //    db.Movies.Remove(g);

                //}

                //Hall hall =
                //    new Hall { Number = 9, Cinema = db.Cinemas.Where(c => c.Name == "General Movie Theater").FirstOrDefault() };
                //db.Halls.Add(hall);
                //db.SaveChanges();


                //Movie movie = new Movie { 
                //  Name = "Titanic", Genre = db.Genres.Where(c=>c.Name =="Drama").FirstOrDefault(), Rating = 8.5 };
                //db.Movies.Add(movie);
                //db.Genres.AddRange(genres);

                //Movie t2 = db.Movies.FirstOrDefault();
                //t2.Name = "Saw"; // изменим название
                //db.Entry(t2).State = EntityState.Modified;
                //// переведем игрока из одной команды в другую
                //db.SaveChanges();



                //Console.WriteLine("Name Genre Rating ");
                //foreach (Movie m in db.Movies.Include(p => p.Genre))
                //{
                //    Console.WriteLine("{0} {1} {2}", m.Name, m.Genre != null ? m.Genre.Name : "", m.Rating);
                //}

                ////db.SaveChanges();
                //Console.WriteLine("Name Films ");
                //foreach (Genre pl in db.Genres.Include(p => p.Movies))
                //{
                //    Console.WriteLine("{0}", pl.Name);
                //    foreach (Movie m in pl.Movies)
                //    {
                //        Console.WriteLine("{0}", m.Name);

                //    }
                //}

                //Console.WriteLine("Name Halls ");
                //foreach (Cinema pl in db.Cinemas.Include(p => p.Halls))
                //{
                //    Console.WriteLine("{0}", pl.Name);
                //    foreach (Hall m in pl.Halls)
                //    {
                //        Console.WriteLine("{0}", m.Number);

                //    }
                //}

                //Console.WriteLine("Hall_Number Movies ");
                //foreach (Hall pl in db.Halls.Include(p => p.Movies))
                //{
                //    Console.WriteLine("{0}", pl.Number);
                //    foreach (Movie m in pl.Movies)
                //    {
                //        Console.WriteLine("{0}", m.Name);

                //    }
                //    Console.WriteLine();
                //}

                //Console.WriteLine(db.Genres.First().Name);
                Console.Read();
            }

        }
    }
}
