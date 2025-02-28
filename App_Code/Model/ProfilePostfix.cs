using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for ProfilePostfix
/// </summary>
public static class Postfix
{
    private static Dictionary<int, ProfilePostfix> ProfilesPostfix = new Dictionary<int, ProfilePostfix>();
    private static Dictionary<int, PostfixMachine> Postfixs = new Dictionary<int, PostfixMachine>();

    public static void Statr()
    {
        ReloadProfilePostfix();
    }
    public static void ReloadProfilePostfix()
    {
        try
        {
            lock (Postfixs)
            {
                lock (ProfilesPostfix)
                {
                    using (MySqlConnection conn = new MySqlConnection(Defaults.ConnStr))
                    {
                        MySqlDataReader dr;
                        conn.Open();
                        string sql = "SELECT ID FROM profilePostfix WHERE isActive = 1 AND isDel = 0";
                        using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                        {

                            ProfilesPostfix.Clear();
                            dr = cmd.ExecuteReader();
                            while (dr.Read())
                            {
                                int ID = 0;
                                if (dr["ID"].StringToInt(out ID, EqualsNum.bigger))
                                {
                                    ProfilesPostfix.Add(ID, new ProfilePostfix(ID));
                                }
                            }
                            dr.Close();


                            foreach (var item in ProfilesPostfix)
                            {
                                List<ProfilePostfix.Machine> MachineList = item.Value.MachineList;
                                int ProfileID = item.Value.ID;

                                cmd.CommandText = "SELECT MachineID FROM profilePostfixmachine WHERE ProfilePostfixID = " + ProfileID;
                                dr = cmd.ExecuteReader();
                                while (dr.Read())
                                {
                                    int MachineID = 0;
                                    if (dr["MachineID"].StringToInt(out MachineID, EqualsNum.bigger))
                                    {
                                        MachineList.Add(new ProfilePostfix.Machine(MachineID));
                                    }
                                }
                                dr.Close();
                            }

                            ProfilesPostfix = ProfilesPostfix.Where(d => d.Value.MachineList.Count != 0).ToDictionary(d => d.Key, d => d.Value);

                            cmd.CommandText = "SELECT ServerID ,ServerEnding,ServerHost,userName,password,ServerPort FROM smtpservers";
                            Postfixs.Clear();
                            dr = cmd.ExecuteReader();
                            while (dr.Read())
                            {
                                int id = int.Parse(dr["ServerID"].ToString());
                                Postfixs.Add(id, new PostfixMachine(id, dr["ServerEnding"].ToString(), dr["ServerHost"].ToString(), dr["userName"].ToString(), dr["password"].ToString(), dr["ServerPort"].ToString()));
                            }
                            dr.Close();

                        }
                        conn.Close();
                    }
                }
            }
        }
        catch (Exception exc)
        {
            //Defaults.ErrorWritingToDB_OR_SendMal(Defaults.SendErrorMailUser, "שגיאה ביצירת פרופיל postfix" + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"), exc.Message, exc.StackTrace, new System.Diagnostics.StackTrace(true).GetFrame(0).GetFileName(), new System.Diagnostics.StackTrace(true).GetFrame(0).GetFileLineNumber());
        }
    }

    public static int getRandomPostfixFromProfile(string ProfileID)
    {
        int PostfixID = 0;
        lock (ProfilesPostfix)
        {
            int profileID = 0;
            if (int.TryParse(ProfileID, out profileID) && profileID > 0)
            {
                var profile = ProfilesPostfix[profileID];
                if (profile.MachineList != null && profile.MachineList.Count > 0)
                {
                    if (profile.MachineList.Count > 1)
                    {
                        PostfixID = profile.MachineList[new Random().Next(profile.MachineList.Count)].MachineID;
                    }
                    else
                    {
                        PostfixID = profile.MachineList[0].MachineID;
                    }
                }
            }
        }
        return PostfixID;
    }
    public static List<int> getAllPostfixFromProfile(string ProfileID)
    {
        List<int> PostfixIDs = new List<int>();
        lock (ProfilesPostfix)
        {
            int profileID = 0;
            if (int.TryParse(ProfileID, out profileID) && profileID > 0 && ProfilesPostfix.ContainsKey(profileID))
            {
                var profile = ProfilesPostfix[profileID];
                if (profile.MachineList != null && profile.MachineList.Count > 0)
                {
                    PostfixIDs = profile.MachineList.Select(d => d.MachineID).ToList();
                }
            }
        }
        return PostfixIDs;
    }
    public static PostfixMachine getPostfixByID(int PostfixID)
    {
        PostfixMachine res = null;
        lock (Postfixs)
        {
            if (Postfixs.ContainsKey(PostfixID))
            {
                res = Postfixs[PostfixID];
            }
        }

        return res;
    }

    public static PostfixMachine getPostfixTestMachineBySite(int SitID)
    {
        int DomainID = Domain.getDomainIDFromSiteID(SitID);

        string ProfileID = Domain.getParamsByDomain(siteParamType.ProfilePostfixID, DomainID);

        PostfixMachine PostfixMachine = getPostfixByID(getRandomPostfixFromProfile(ProfileID));
        return PostfixMachine = PostfixMachine ?? new PostfixMachine();
    }

    public static List<int> getPostfixsID_PerSendCount(SortBy sortBy = SortBy.ASC)
    {
        lock (Postfixs)
        {
            return Postfixs.OrderBy(d => d.Value.SendCount).Select(s => s.Key).ToList();
        }
    }

    public static Dictionary<int, PostfixMachine> GetPostfixs()
    {
        lock (Postfixs)
        {
            return Postfixs;
        }
    }
    public enum SortBy
    {
        ASC,
        DESC
    }
    public class ProfilePostfix
    {
        public int ID { get; set; }
        public List<Machine> MachineList { get; set; }
        public ProfilePostfix()
        {

        }
        public ProfilePostfix(int id)
        {
            ID = id;
            MachineList = new List<Machine>();
        }

        public class Machine
        {
            public int MachineID { get; set; }
            public Machine()
            {

            }
            public Machine(int machineID)
            {
                MachineID = machineID;
            }
        }
    }

    public class PostfixMachine
    {
        public int ID { get; set; }
        public string Ending { get; set; }
        public string Host { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int Port { get; set; }
        public long SendCount { get; set; }
        public PostfixMachine(int id, string ending, string host, string userName, string password, string port)
        {
            ID = id;
            Ending = ending;
            Host = host;
            UserName = userName;
            Password = password;
            Port = int.Parse(port);
        }
        public PostfixMachine()
        {

        }
    }
}