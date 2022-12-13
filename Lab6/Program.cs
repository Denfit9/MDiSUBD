using Mdisubc;
using MySql.Data.MySqlClient;
using System.Data.Common;

Console.WriteLine("Getting Connection ...");
MySqlConnection conn = DBUtils.GetDBConnection();

try
{
    Console.WriteLine("Openning Connection ...");

    conn.Open();

    Console.WriteLine("Connection successful!");
}
catch (Exception e)
{
    Console.WriteLine("Error: " + e.Message);
}
//got connection

Boolean exit = false;
int level = 0;
int cartId = 0;
int price = 0;

/*
level 0 = starting page
level 1 = login page
level 2 = register page
level 3 = admin page
level 4 = customer page
level 5 = admin ticket page
level 6 = customer ticket page
level 7 = exact ticket
level 8 = customers cart page
level 9 = exact ticket in cart
level 10 = history of bought tickets
level 11 = exact bought ticket
*/

int role = 0;
/* role 1 = admin
   role 2 = customer*/

int userId = 0;


while (exit != true)
{
    if (level == 0)
    {
        Console.WriteLine("Type what do you want to do\n 1 - Login \n 2 - Register \n c - close the app\n");
        char action;
        action = Console.ReadKey().KeyChar;
        Console.WriteLine("\n");
        switch (action)
        {
            case '1':
                level = 1;
                break;
            case '2':
                level = 2;
                break;

            case 'c':
                exit = true;
                break;
            default:
                break;
        }
    }
    else if (level == 1)
    {
        Console.Clear();
        //authorize(conn, "12345", "someadmin@gmail.com", level);
        Console.WriteLine("Enter a password\n");

        string enterPassword = Console.ReadLine();
        while (enterPassword == "")
        {
            Console.WriteLine("\nEmpty line is not allowed here\nEnter a password\n");
            enterPassword = Console.ReadLine();
        }
        Console.WriteLine("Enter an email\n");
        string enterEmail = Console.ReadLine();
        while (enterEmail == "")
        {
            Console.WriteLine("\nEmpty line is not allowed here\nEnter an email\n");
            enterEmail = Console.ReadLine();
        }
        authorize(conn, enterPassword, enterEmail, ref level, ref role, ref userId, ref cartId, ref price);

        //Console.ReadLine();
    }
    else if (level == 2)
    {
        register(conn, ref level, ref role, ref userId, ref cartId);
        Console.Clear();

    }
    else if (level == 3)
    {
        Console.Clear();
        Console.WriteLine("Welcome admin");
        Console.WriteLine("Type what do you want to do\n 1 - Check the logs \n 2 - See all tickets \n l - logout \n c - close the app\n");
        char adminAction = Console.ReadKey().KeyChar;
        Console.WriteLine("\n");
        switch (adminAction)
        {
            case '1':
                //level = 1;
                break;

            case 'l':
                level = 0;
                role = 0;
                userId = 0;
                Console.Clear();
                break;

            case 'c':
                exit = true;
                break;
            default:
                break;
        }
    }
    else if (level == 4)
    {

        Console.WriteLine("Welcome customer");
        Console.WriteLine("Type what do you want to do\n 1 - Check all available tickets \n 2 - Check the cart \n 3 - Check bought tickets" +
            "\n 4 - See all possible routes \n l - logout \n c - close the app\n");
        char customerAction = Console.ReadKey().KeyChar;
        Console.WriteLine("\n");
        switch (customerAction)
        {
            case '1':
                level = 6;
                break;

            case '2':
                level = 8;
                break;
            case '3':
                level = 10;
                break;
            case '4':
                getAllPossibleRoutes(conn);
                break;

            case 'l':
                level = 0;
                role = 0;
                userId = 0;
                Console.Clear();
                break;

            case 'c':
                exit = true;
                break;
            default:
                break;
        }
    }
    else if (level == 6)
    {
        Console.Clear();
        Console.WriteLine("Tickets are:\n");
        getAllTicketsCustomer(conn);
        Console.WriteLine("Type what do you want to do\n 1 - Look exact ticket \n 2 - Look exact ticket \n b - back \n c - close the app\n");
        char customerAction = Console.ReadKey().KeyChar;
        Console.WriteLine("\n");
        switch (customerAction)
        {
            case '1':
                // GetAllTicketsCustomer(conn);
                // level = 6;
                level = 7;
                break;

            case 'b':
                level = 4;
                Console.Clear();
                break;

            case 'c':
                exit = true;
                break;
            default:
                break;
        }

    }
    else if (level == 7)
    {
        Console.Clear();
        Console.WriteLine("Type number of the ticket you want to see");
        bool isTicketChosen = false;
        int ticketNumber = Convert.ToInt32(Console.ReadLine());
        isTicketChosen = getMoreInfoAboutTicket(ticketNumber);

        if(isTicketChosen!= false)
        {
            Console.WriteLine("What to do next\n 1 - add ticket to cart \n b - back\n r - repeat input\n");
            char customerAction = Console.ReadKey().KeyChar;
            Console.WriteLine("\n");
            switch (customerAction)
            {
                case '1':
                    addicketToCart(conn, userId, cartId, ref price, ref ticketNumber);
                    // GetAllTicketsCustomer(conn);
                    level = 4;
                    ticketNumber = 0;
                    isTicketChosen = false;
                    break;


                case 'b':
                    level = 6;
                    isTicketChosen = false;
                    ticketNumber = 0;
                    Console.Clear();
                    break;

                case 'c':
                    exit = true;
                    break;
                default:
                    break;
            }
        }
        else
        {
            Console.WriteLine("What to do next\n\n b - back\n r - repeat input\n");
            char customerAction = Console.ReadKey().KeyChar;
            Console.WriteLine("\n");
            switch (customerAction)
            {
                case 'b':
                    level = 6;
                    isTicketChosen = false;
                    ticketNumber = 0;
                    Console.Clear();
                    break;

                case 'c':
                    exit = true;
                    break;
                default:
                    break;
            }
        }
        

    }
    else if (level == 8)
    {
        Console.Clear();
        Console.WriteLine("Total price in cart is: " + price + "\nCart contains such tickets:\n");
        getAllTicketsInCart(conn, ref userId, ref cartId);
        Console.WriteLine("Type what do you want to do\n 1 - Look exact ticket \n 2 - Buy the cart \n 3 - Drop all tickets from cart \n b - back \n c - close the app\n");
        char customerAction = Console.ReadKey().KeyChar;
        Console.WriteLine("\n");
        switch (customerAction)
        {
            case '1':
                // GetAllTicketsCustomer(conn);
                // level = 6;
                level = 9;
                break;
            case '2':
                buyTicketsFromCart(conn, userId,  cartId, ref price);
                level = 4;
                Console.Clear();
                Console.WriteLine("Everything bought from cart. Thank you for your purchase!");
                break;
            case '3':
                dropAllTicketsFromCart(conn, userId, cartId, ref price);
                level = 4;
                Console.Clear();
                Console.WriteLine("Cart is free and every ticket is dropped now!");
                break;

            case 'b':
                level = 4;
                Console.Clear();
                break;

            case 'c':
                exit = true;
                break;
            default:
                break;
        }

    }
    else if (level == 9)
    {
        Console.Clear();
        Console.WriteLine("Type number of the ticket you want to see");
        int ticketNumber = Convert.ToInt32(Console.ReadLine());
        bool ticketChosen = false;
        ticketChosen =  getMoreInfoAboutTicketInCart(ticketNumber, cartId);

        if (ticketChosen != false)
        {
            Console.WriteLine("What to do next\n 1 - drop ticket \n b - back\n r - repeat input\n");
            char customerAction = Console.ReadKey().KeyChar;
            Console.WriteLine("\n");
            switch (customerAction)
            {
                case '1':
                    dropTicketFromCart(conn, userId, cartId, ref price, ref ticketNumber);
                    // GetAllTicketsCustomer(conn);
                    level = 8;
                    ticketNumber = 0;
                    ticketChosen = false;
                    break;


                case 'b':
                    level = 8;
                    ticketChosen = false;
                    ticketNumber= 0;
                    Console.Clear();
                    break;

                case 'c':
                    exit = true;
                    break;
                default:
                    break;
            }
        }
        else
        {
            Console.WriteLine("What to do next \n b - back\n r - repeat input\n");
            char customerAction = Console.ReadKey().KeyChar;
            Console.WriteLine("\n");
            switch (customerAction)
            {
                case '1':
                    // GetAllTicketsCustomer(conn);
                    // level = 6;
                    break;


                case 'b':
                    level = 8;
                    ticketChosen = false;
                    ticketNumber = 0;
                    Console.Clear();
                    break;

                case 'c':
                    exit = true;
                    break;
                default:
                    break;
            }

        }
    }
    else if (level == 10)
    {
        Console.Clear();
        Console.WriteLine("Tickets are:\n");
        getAllTicketsBoughtCustomer(conn, cartId);
        Console.WriteLine("Type what do you want to do\n 1 - Look exact ticket  \n b - back \n c - close the app\n");
        char customerAction = Console.ReadKey().KeyChar;
        Console.WriteLine("\n");
        switch (customerAction)
        {
            case '1':
                // GetAllTicketsCustomer(conn);
                // level = 6;
                level = 11;
                break;

            case 'b':
                level = 4;
                Console.Clear();
                break;

            case 'c':
                exit = true;
                break;
            default:
                break;
        }
    }
    else if(level == 11)
    {
        Console.Clear();
        Console.WriteLine("Type number of the ticket you want to see");
        int ticketNumber = Convert.ToInt32(Console.ReadLine());
        bool ticketChosen = false;
        ticketChosen = getMoreInfoAboutBoughtTicket(ticketNumber, cartId);

        if (ticketChosen != false)
        {
            Console.WriteLine("What to do next\n b - back\n r - repeat input\n");
            char customerAction = Console.ReadKey().KeyChar;
            Console.WriteLine("\n");
            switch (customerAction)
            {
                case 'b':
                    level = 10;
                    ticketChosen = false;
                    ticketNumber = 0;
                    Console.Clear();
                    break;

                case 'c':
                    exit = true;
                    break;
                default:
                    break;
            }
        }
        else
        {
            Console.WriteLine("What to do next \n b - back\n r - repeat input\n");
            char customerAction = Console.ReadKey().KeyChar;
            Console.WriteLine("\n");
            switch (customerAction)
            {
                case 'b':
                    level = 8;
                    ticketChosen = false;
                    ticketNumber = 0;
                    Console.Clear();
                    break;

                case 'c':
                    exit = true;
                    break;
                default:
                    break;
            }

        }
    }


    void register(MySqlConnection conn, ref int levelchange, ref int role, ref int userId, ref int cartId)
    {
        Console.WriteLine("Enter an email");
        string email = Console.ReadLine();

        MySqlCommand cmd = new MySqlCommand();
        cmd.Connection = conn;
        string isEmailBusy = "Select EXISTS(SELECT * FROM userbase WHERE email = '" + email + "')";
        cmd.CommandText = isEmailBusy;
        object objEmail = cmd.ExecuteScalar();

        while (Convert.ToInt32(objEmail) > 0 || email == "")
        {
            Console.WriteLine("This email is busy, sorry");
            Console.WriteLine("Enter an email");
            email = Console.ReadLine();
            isEmailBusy = "Select EXISTS(SELECT * FROM userbase WHERE email = '" + email + "')";
            cmd.CommandText = isEmailBusy;
            objEmail = cmd.ExecuteScalar();
        }
        Console.WriteLine("Enter a password");
        string password = Console.ReadLine();
        while (password == "")
        {
            Console.WriteLine("You can't leave your password empty! Try again");
            password = Console.ReadLine();
        }
        Console.WriteLine("Enter your city of dislocation");
        string city = Console.ReadLine();
        while (city == "")
        {
            Console.WriteLine("You can't leave your city empty! Try again");
            city = Console.ReadLine();
        }
        Console.WriteLine("Enter your name");
        string name = Console.ReadLine();
        while (name == "")
        {
            Console.WriteLine("You definitely have a name! Try again");
            name = Console.ReadLine();
        }
        Console.WriteLine("Enter your surname");
        string surname = Console.ReadLine();
        while (surname == "")
        {
            Console.WriteLine("You definitely have a surname! Try again");
            surname = Console.ReadLine();
        }
        string addUserbase = "call InsertUserbase('" + email + "','" + password + "')";
        cmd.CommandText = addUserbase;
        int number = cmd.ExecuteNonQuery();
        string getUserId = "Select UserId From userbase where email = '" + email + "' and password = '" + password + "'";
        cmd.CommandText = getUserId;
        userId = Convert.ToInt32(cmd.ExecuteScalar());
        string insertCustomer = "call InsertCustomer('" + city + "', '" + name + "', '" + surname + "', " + userId + ");";
        cmd.CommandText = insertCustomer;
        int ins = cmd.ExecuteNonQuery();
        string getCartId = "Select cartId from cart join customer using(CustomerId) where userId = " + userId;
        cmd.CommandText = getCartId;
        cartId = Convert.ToInt32(cmd.ExecuteScalar());
        levelchange = 4;
        role = 2;
        Console.Clear();
        Console.WriteLine("Created succesfully\n Welcome new customer\n");
    }


    void authorize(MySqlConnection conn, string userpassword, string email, ref int levelchange, ref int role, ref int userId, ref int cartId, ref int price)
    {
        string sql = "Select EXISTS(SELECT * FROM USERBASE WHERE password = '" + userpassword + "' and email ='" + email + "')";
        MySqlCommand cmd = new MySqlCommand();
        cmd.Connection = conn;
        cmd.CommandText = sql;

        object obj = cmd.ExecuteScalar();

        if (Convert.ToInt32(obj) > 0)
        {
            string getUserId = "Select UserId FROM userbase WHERE password = '" + userpassword + "' and email ='" + email + "'";
            cmd.CommandText = getUserId;

            using (DbDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    userId = reader.GetInt32(0);
                }
            }

            string isAdmin = "Select EXISTS(SELECT * FROM administrator WHERE userId = '" + userId + "')";
            cmd.CommandText = isAdmin;
            object objAdmin = cmd.ExecuteScalar();

            string isCustomer = "Select EXISTS(SELECT * FROM customer WHERE userId = '" + userId + "')";
            cmd.CommandText = isCustomer;
            object objCustomer = cmd.ExecuteScalar();

            if (Convert.ToInt32(objAdmin) > 0)
            {
                levelchange = 3;
                role = 1;
            }
            else if (Convert.ToInt32(objCustomer) > 0)
            {
                levelchange = 4;
                role = 2;

                string getCartId = "Select cartId from cart join customer using(CustomerId) where userId = " + userId + "";
                cmd.CommandText = getCartId;

                using (DbDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        cartId = reader.GetInt32(0);
                    }
                }

                string getCartPrice = "Select TotalPrice from cart join customer using(CustomerId) where userId = " + userId + "";
                cmd.CommandText = getCartPrice;

                using (DbDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        price = reader.GetInt32(0);
                    }
                }
            }
            else
            {
                levelchange = 0;
                role = 0;
                userId = 0;
            }
        }
        else
        {
            Console.Clear();
            Console.WriteLine("Such user doesn't exist");
            levelchange = 0;
        }
        Console.Clear();
    }



    void getAllTickets(MySqlConnection conn)
    {
        string sql = "Select * from ticket";

        MySqlCommand cmd = new MySqlCommand();
        cmd.Connection = conn;
        cmd.CommandText = sql;

        using (DbDataReader reader = cmd.ExecuteReader())
        {
            if (reader.HasRows)
            {

                while (reader.Read())
                {
                    int ticketId = reader.GetInt32(0);
                    string startingPoint = reader.GetString(1);
                    string destinationPoint = reader.GetString(2);
                    int ticketPrice = reader.GetInt32(3);
                    int trainId = reader.GetInt32(4);
                    string departureTime = Convert.ToString(reader.GetDateTime(5));
                    string arrivingTime = Convert.ToString(reader.GetDateTime(6));
                    string isBought = Convert.ToString(reader.GetBoolean(7));
                    int seatNumber = reader.GetInt32(8);
                    String cartId;
                    if (!reader.IsDBNull(9))
                    {
                        cartId = Convert.ToString(reader.GetInt32(9));
                    }
                    else
                    {
                        cartId = "Null";
                    }

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("--------------------");
                    Console.ResetColor();
                    Console.WriteLine("Ticket Id: " + ticketId);
                    Console.WriteLine("Starting point: " + startingPoint);
                    Console.WriteLine("Destination point: " + destinationPoint);
                    Console.WriteLine("Price: " + ticketPrice);
                    Console.WriteLine("trainId: " + trainId);
                    Console.WriteLine("Departure time: " + departureTime);
                    Console.WriteLine("Arriving time: " + arrivingTime);
                    Console.WriteLine("Is ticket bought: " + isBought);
                    Console.WriteLine("Number of seat: " + seatNumber);
                    Console.WriteLine("Cart Id: " + cartId);
                    Console.WriteLine("\n\n");
                }
            }
        }
    }

    void getAllTicketsInCart(MySqlConnection conn, ref int userId, ref int cartId)
    {
        string sql = "Select * from ticket  join train using(TrainId) Where CartId = " + cartId + " and isBought = 0";
        MySqlCommand cmd = new MySqlCommand();
        cmd.Connection = conn;
        cmd.CommandText = sql;
        using (DbDataReader reader = cmd.ExecuteReader())
        {


            while (reader.Read())
            {
                int ticketId = reader.GetInt32(1);
                string startingPoint = reader.GetString(2);
                string destinationPoint = reader.GetString(3);// 2
                int ticketPrice = reader.GetInt32(4);
                int trainId = reader.GetInt32(0);
                string departureTime = Convert.ToString(reader.GetDateTime(5));
                string arrivingTime = Convert.ToString(reader.GetDateTime(6));
                string isBought = Convert.ToString(reader.GetBoolean(7));
                int seatNumber = reader.GetInt32(8);

                string trainDescription = reader.GetString(10);


                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("--------------------");
                Console.ResetColor();
                Console.WriteLine("Ticket Id: " + ticketId);
                Console.WriteLine("Train Id: " + trainId);
                Console.WriteLine("Starting point: " + startingPoint);
                Console.WriteLine("Destination point: " + destinationPoint);
                Console.WriteLine("Price: " + ticketPrice);
                Console.WriteLine("Departure time: " + departureTime);
                Console.WriteLine("Arriving time: " + arrivingTime);
                Console.WriteLine("Number of seat: " + seatNumber);
                Console.WriteLine("Train description: " + trainDescription);
                Console.WriteLine("");
            }
        }
    }

    void getAllTicketsCustomer(MySqlConnection conn)
    {
        string sql = "Select * from ticket  join train using(TrainId) Where CartId Is Null";

        MySqlCommand cmd = new MySqlCommand();
        cmd.Connection = conn;
        cmd.CommandText = sql;

        using (DbDataReader reader = cmd.ExecuteReader())
        {


            while (reader.Read())
            {
                int ticketId = reader.GetInt32(1);
                string startingPoint = reader.GetString(2);
                string destinationPoint = reader.GetString(3);// 2
                int ticketPrice = reader.GetInt32(4);
                int trainId = reader.GetInt32(0);
                string departureTime = Convert.ToString(reader.GetDateTime(5));
                string arrivingTime = Convert.ToString(reader.GetDateTime(6));
                string isBought = Convert.ToString(reader.GetBoolean(7));
                int seatNumber = reader.GetInt32(8);
                String cartId;
                if (!reader.IsDBNull(9))
                {
                    cartId = Convert.ToString(reader.GetInt32(9));
                }
                else
                {
                    cartId = "Null";
                }
                string trainDescription = reader.GetString(10);


                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("--------------------");
                Console.ResetColor();
                Console.WriteLine("Ticket Id: " + ticketId);
                Console.WriteLine("Train Id: " + trainId);
                Console.WriteLine("Starting point: " + startingPoint);
                Console.WriteLine("Destination point: " + destinationPoint);
                Console.WriteLine("Price: " + ticketPrice);
                Console.WriteLine("Departure time: " + departureTime);
                Console.WriteLine("Arriving time: " + arrivingTime);
                Console.WriteLine("Number of seat: " + seatNumber);
                Console.WriteLine("Train description: " + trainDescription);
                Console.WriteLine("");
            }

        }
    }

    bool getMoreInfoAboutTicket(int ticketId)
    {
        string sql = "Select * from ticket  join train using(TrainId) Where ticketId = " + ticketId;
        int trainId = 0;
        MySqlCommand cmd = new MySqlCommand();
        cmd.Connection = conn;

        string isTicket = "Select exists(Select * from ticket where TicketId = " + ticketId + " and CartId is Null)";
        cmd.CommandText = isTicket;
        object objTicket = cmd.ExecuteScalar();

        if (Convert.ToInt32(objTicket) > 0)
        {
            cmd.CommandText = sql;
            using (DbDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    string startingPoint = reader.GetString(2);
                    string destinationPoint = reader.GetString(3);
                    int ticketPrice = reader.GetInt32(4);
                    trainId = reader.GetInt32(0);
                    string departureTime = Convert.ToString(reader.GetDateTime(5));
                    string arrivingTime = Convert.ToString(reader.GetDateTime(6));
                    string isBought = Convert.ToString(reader.GetBoolean(7));
                    int seatNumber = reader.GetInt32(8);
                    String cartId;
                    if (!reader.IsDBNull(9))
                    {
                        cartId = Convert.ToString(reader.GetInt32(9));
                    }
                    else
                    {
                        cartId = "Null";
                    }
                    string trainDescription = reader.GetString(10);

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("--------------------");
                    Console.ResetColor();
                    Console.WriteLine("Ticket Id: " + ticketId);
                    Console.WriteLine("Train Id: " + trainId);
                    Console.WriteLine("Starting point: " + startingPoint);
                    Console.WriteLine("Destination point: " + destinationPoint);
                    Console.WriteLine("Price: " + ticketPrice);
                    Console.WriteLine("Departure time: " + departureTime);
                    Console.WriteLine("Arriving time: " + arrivingTime);
                    Console.WriteLine("Number of seat: " + seatNumber);
                    Console.WriteLine("Train description: " + trainDescription);

                }
            }


            string getCarriage = "Select * from carriage join traincomplectation using(carriageID) where trainId = " + trainId;
            cmd.CommandText = getCarriage;
            Console.WriteLine("Carriage: ");
            using (DbDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    string carriage = reader.GetString(1);
                    Console.Write(carriage + " ");
                }
                Console.WriteLine("");
            }
            string getTypeOfTrain = "Select * from traintype join traintypeconnection using(trainTypeID) where trainId = " + trainId;
            cmd.CommandText = getTypeOfTrain;
            using (DbDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    string trainType = reader.GetString(1);
                    Console.Write("Train type is: " + trainType);
                }
                Console.WriteLine("\n");
            }
            return true;
        }
        else
        {
            Console.WriteLine("No such Ticket exists, sorry\n");
            return false;
        }
    }

    bool getMoreInfoAboutTicketInCart(int ticketId, int userCartId)
    {
        string sql = "Select * from ticket  join train using(TrainId) Where ticketId = " + ticketId + " and cartId = " + userCartId;
        int trainId = 0;
        MySqlCommand cmd = new MySqlCommand();
        cmd.Connection = conn;

        string isTicket = "Select exists(Select * from ticket where TicketId = " + ticketId + " and CartId = " + userCartId  + " and isBought = 0"  +")";
        cmd.CommandText = isTicket;
        object objTicket = cmd.ExecuteScalar();

        if (Convert.ToInt32(objTicket) > 0)
        {
            cmd.CommandText = sql;
            using (DbDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    string startingPoint = reader.GetString(2);
                    string destinationPoint = reader.GetString(3);
                    int ticketPrice = reader.GetInt32(4);
                    trainId = reader.GetInt32(0);
                    string departureTime = Convert.ToString(reader.GetDateTime(5));
                    string arrivingTime = Convert.ToString(reader.GetDateTime(6));
                    string isBought = Convert.ToString(reader.GetBoolean(7));
                    int seatNumber = reader.GetInt32(8);
                    String cartId;
                    if (!reader.IsDBNull(9))
                    {
                        cartId = Convert.ToString(reader.GetInt32(9));
                    }
                    else
                    {
                        cartId = "Null";
                    }
                    string trainDescription = reader.GetString(10);

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("--------------------");
                    Console.ResetColor();
                    Console.WriteLine("Ticket Id: " + ticketId);
                    Console.WriteLine("Train Id: " + trainId);
                    Console.WriteLine("Starting point: " + startingPoint);
                    Console.WriteLine("Destination point: " + destinationPoint);
                    Console.WriteLine("Price: " + ticketPrice);
                    Console.WriteLine("Departure time: " + departureTime);
                    Console.WriteLine("Arriving time: " + arrivingTime);
                    Console.WriteLine("Number of seat: " + seatNumber);
                    Console.WriteLine("Train description: " + trainDescription);

                }
            }


            string getCarriage = "Select * from carriage join traincomplectation using(carriageID) where trainId = " + trainId;
            cmd.CommandText = getCarriage;
            Console.WriteLine("Carriage: ");
            using (DbDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    string carriage = reader.GetString(1);
                    Console.Write(carriage + " ");
                }
                Console.WriteLine("");
            }
            string getTypeOfTrain = "Select * from traintype join traintypeconnection using(trainTypeID) where trainId = " + trainId;
            cmd.CommandText = getTypeOfTrain;
            using (DbDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    string trainType = reader.GetString(1);
                    Console.Write("Train type is: " + trainType);
                }
                Console.WriteLine("\n");
            }
            return true;
        }
        else
        {
            Console.WriteLine("You don't own such ticket sorry\n");
            return false;
        }
    }

    bool getMoreInfoAboutBoughtTicket(int ticketId, int userCartId)
    {
        string sql = "Select * from ticket  join train using(TrainId) Where ticketId = " + ticketId;
        int trainId = 0;
        MySqlCommand cmd = new MySqlCommand();
        cmd.Connection = conn;

        string isTicket = "Select exists(Select * from ticket where TicketId = " + ticketId + " and CartId = " + userCartId + " and isBought = 1" + ")";
        cmd.CommandText = isTicket;
        object objTicket = cmd.ExecuteScalar();

        if (Convert.ToInt32(objTicket) > 0)
        {
            cmd.CommandText = sql;
            using (DbDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    string startingPoint = reader.GetString(2);
                    string destinationPoint = reader.GetString(3);
                    int ticketPrice = reader.GetInt32(4);
                    trainId = reader.GetInt32(0);
                    string departureTime = Convert.ToString(reader.GetDateTime(5));
                    string arrivingTime = Convert.ToString(reader.GetDateTime(6));
                    string isBought = Convert.ToString(reader.GetBoolean(7));
                    int seatNumber = reader.GetInt32(8);
                    String cartId;
                    if (!reader.IsDBNull(9))
                    {
                        cartId = Convert.ToString(reader.GetInt32(9));
                    }
                    else
                    {
                        cartId = "Null";
                    }
                    string trainDescription = reader.GetString(10);

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("--------------------");
                    Console.ResetColor();
                    Console.WriteLine("Ticket Id: " + ticketId);
                    Console.WriteLine("Train Id: " + trainId);
                    Console.WriteLine("Starting point: " + startingPoint);
                    Console.WriteLine("Destination point: " + destinationPoint);
                    Console.WriteLine("Price: " + ticketPrice);
                    Console.WriteLine("Departure time: " + departureTime);
                    Console.WriteLine("Arriving time: " + arrivingTime);
                    Console.WriteLine("Number of seat: " + seatNumber);
                    Console.WriteLine("Train description: " + trainDescription);

                }
            }


            string getCarriage = "Select * from carriage join traincomplectation using(carriageID) where trainId = " + trainId;
            cmd.CommandText = getCarriage;
            Console.WriteLine("Carriage: ");
            using (DbDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    string carriage = reader.GetString(1);
                    Console.Write(carriage + " ");
                }
                Console.WriteLine("");
            }
            string getTypeOfTrain = "Select * from traintype join traintypeconnection using(trainTypeID) where trainId = " + trainId;
            cmd.CommandText = getTypeOfTrain;
            using (DbDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    string trainType = reader.GetString(1);
                    Console.Write("Train type is: " + trainType);
                }
                Console.WriteLine("\n");
            }
            return true;
        }
        else
        {
            Console.WriteLine("You didn't buy such ticket\n");
            return false;
        }
    }

    void dropTicketFromCart(MySqlConnection conn, int userId, int cartId, ref int price, ref int ticketId)
    {
        string sql = "call DropTicket(" + userId + ","  + ticketId + ")";

        MySqlCommand cmd = new MySqlCommand();
        cmd.Connection = conn;
        cmd.CommandText = sql;
        object objBuy = cmd.ExecuteScalar();
        int ticketPrice = getTicketPrice(conn, ticketId);
        string updatePrice = "call LowerCartPrice(" + ticketPrice + "," + cartId + ")";
        cmd.CommandText = updatePrice;
        object objUpdatePrice = cmd.ExecuteScalar();
        price -= ticketPrice;
        string updateLog = "call UpdateLog(" + userId + ",'Ticket dropped', 'Customer dropped  ticket from cart with TicketId =" + cartId + "')";
        cmd.CommandText = updateLog;
        object objLog = cmd.ExecuteScalar();
        Console.Clear();
        Console.WriteLine("Ticket dropped!");
    }

    void buyTicketsFromCart(MySqlConnection conn, int userId, int cartId, ref int price)
    {
        string sql = "call ZeroCartPrice(" + cartId + ")";

        MySqlCommand cmd = new MySqlCommand();
        cmd.Connection = conn;
        cmd.CommandText = sql;
        object objBuy = cmd.ExecuteScalar();
        price = 0;
        string updateLog = "call UpdateLog(" + userId + ",'Cart bought', 'Customer just bought his cart, where cartId = " + cartId + "')";
        cmd.CommandText = updateLog;
        object objLog = cmd.ExecuteScalar();
    }

    void dropAllTicketsFromCart(MySqlConnection conn, int userId, int cartId, ref int price)
    {
        string sql = "call FreeCart(" + cartId + ")";

        MySqlCommand cmd = new MySqlCommand();
        cmd.Connection = conn;
        cmd.CommandText = sql;
        object objBuy = cmd.ExecuteScalar();
        price = 0;
        string updateLog = "call UpdateLog(" + userId + ",'Cart emptied', 'Customer just emptied his cart, where cartId = " + cartId + "')";
        cmd.CommandText = updateLog;
        object objLog = cmd.ExecuteScalar();
    }

    void addicketToCart(MySqlConnection conn, int userId, int cartId, ref int price, ref int ticketId)
    {
        string sql = "call BuyTicket(" + userId +"," + cartId + "," + ticketId + ")";

        MySqlCommand cmd = new MySqlCommand();
        cmd.Connection = conn;
        cmd.CommandText = sql;
        object objBuy = cmd.ExecuteScalar();
        int ticketPrice = getTicketPrice(conn, ticketId);
        string updatePrice = "call RaiseCartPrice(" + ticketPrice + "," + cartId + ")";
        cmd.CommandText = updatePrice;
        object objUpdatePrice = cmd.ExecuteScalar();
        price += ticketPrice;
        string updateLog = "call UpdateLog(" + userId + ",'Ticket bought', 'Customer bought ticket with TicketId =" + cartId + "')";
        cmd.CommandText = updateLog;
        object objLog = cmd.ExecuteScalar();
        Console.Clear();
        Console.WriteLine("Ticket added to cart!");
    }

    int getTicketPrice(MySqlConnection conn, int ticketId)
    {
        string sql = "Select Price from ticket where ticketId = " + ticketId;
        MySqlCommand cmd = new MySqlCommand();
        cmd.Connection = conn;
        cmd.CommandText= sql;
        object objBuy = cmd.ExecuteScalar();
        return Convert.ToInt32(objBuy);
    }

    void getAllTicketsBoughtCustomer(MySqlConnection conn, int userCartId)
    {
        string sql = "Select * from ticket where isBought = 1 and cartId =" + userCartId;

        MySqlCommand cmd = new MySqlCommand();
        cmd.Connection = conn;
        cmd.CommandText = sql;

        using (DbDataReader reader = cmd.ExecuteReader())
        {
            if (reader.HasRows)
            {

                while (reader.Read())
                {
                    int ticketId = reader.GetInt32(0);
                    string startingPoint = reader.GetString(1);
                    string destinationPoint = reader.GetString(2);
                    int ticketPrice = reader.GetInt32(3);
                    int trainId = reader.GetInt32(4);
                    string departureTime = Convert.ToString(reader.GetDateTime(5));
                    string arrivingTime = Convert.ToString(reader.GetDateTime(6));
                    int seatNumber = reader.GetInt32(8);
                    

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("--------------------");
                    Console.ResetColor();
                    Console.WriteLine("Ticket Id: " + ticketId);
                    Console.WriteLine("Starting point: " + startingPoint);
                    Console.WriteLine("Destination point: " + destinationPoint);
                    Console.WriteLine("Price: " + ticketPrice);
                    Console.WriteLine("trainId: " + trainId);
                    Console.WriteLine("Departure time: " + departureTime);
                    Console.WriteLine("Arriving time: " + arrivingTime);
                    Console.WriteLine("Number of seat: " + seatNumber);
                    Console.WriteLine("\n\n");
                }
            }
        }
    }


    void getAllPossibleRoutes(MySqlConnection conn)
    {
        string sql = "Select  StartingPoint, DestinationPoint from ticket Group by  DestinationPoint;";

        MySqlCommand cmd = new MySqlCommand();
        cmd.Connection = conn;
        cmd.CommandText = sql;

        using (DbDataReader reader = cmd.ExecuteReader())
        {
            if (reader.HasRows)
            {

                while (reader.Read())
                {
                    
                    string startingPoint = reader.GetString(0);
                    string destinationPoint = reader.GetString(1);

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("--------------------");
                    Console.ResetColor();
                    Console.WriteLine("Starting point: " + startingPoint);
                    Console.WriteLine("Destination point: " + destinationPoint);

                    Console.WriteLine("\n\n");
                }
            }
        }
    }
}


