using System;
using Server;
using Server.Items;
using Server.Network;
using Server.Mobiles;
using Server.Spells;
using System.Collections;

namespace Server.Engines.XmlSpawner2
{
    public class XmlSummonStrike : XmlAttachment
    {
        private int m_Chance = 5;       // 5% chance by default
        private TimeSpan m_Refractory = TimeSpan.FromSeconds(5);    // 5 seconds default time between activations
        private DateTime m_EndTime;
        private string m_Minion = "Drake";
        private ArrayList MinionList = new ArrayList();

        [CommandProperty(AccessLevel.GameMaster)]
        public int Chance { get { return m_Chance; } set { m_Chance = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public TimeSpan Refractory { get { return m_Refractory; } set { m_Refractory = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public string Minion { get { return m_Minion; } set { m_Minion = value; } }

        // Serial constructor is REQUIRED
        public XmlSummonStrike(ASerial serial) : base(serial)
        {
        }

        // Overloads for constructor
        [Attachable]
        public XmlSummonStrike(string minion)
        {
            m_Minion = minion;
            Expiration = TimeSpan.FromMinutes(30);
        }

        [Attachable]
        public XmlSummonStrike(string minion, int chance)
        {
            m_Chance = chance;
            m_Minion = minion;
            Expiration = TimeSpan.FromMinutes(30);
        }

        [Attachable]
        public XmlSummonStrike(string minion, int chance, double refractory)
        {
            m_Chance = chance;
            Refractory = TimeSpan.FromSeconds(refractory);
            Expiration = TimeSpan.FromMinutes(30);
            m_Minion = minion;
        }

        [Attachable]
        public XmlSummonStrike(string minion, int chance, double refractory, double expiresin)
        {
            m_Chance = chance;
            Expiration = TimeSpan.FromMinutes(expiresin);
            Refractory = TimeSpan.FromSeconds(refractory);
            m_Minion = minion;
        }

        public override void OnAttach()
        {
            base.OnAttach();
            if (AttachedTo is Mobile)
            {
                Mobile m = (Mobile)AttachedTo;
                Effects.PlaySound(m, m.Map, 516);
            }
        }

        public override void OnWeaponHit(Mobile attacker, Mobile defender, BaseWeapon weapon, int damageGiven)
        {
            // if it is still refractory then return
            if (DateTime.Now < m_EndTime) return;

            if (m_Chance <= 0 || Utility.Random(100) > m_Chance) return;

            if (attacker is PlayerMobile && defender != null)
            {
                PlayerMobile player = attacker as PlayerMobile;
                
                // check for available control slots
                if (player.Followers + 1 <= player.FollowersMax)
                {
                    // spawn a minion
                    object o = null;
                    try
                    {
                        o = Activator.CreateInstance(SpawnerType.GetType(m_Minion));
                    }
                    catch { }

                    if (o is BaseCreature)
                    {
                        BaseCreature b = (BaseCreature)o;
                        b.MoveToWorld(attacker.Location, attacker.Map);
                        b.Controlled = true;
                        b.ControlMaster = attacker;
                        b.Combatant = defender;

                        // add it to the list of controlled mobs
                        MinionList.Add(b);
                    }
                    else
                    {
                        // Clean up if not a BaseCreature
                        if (o is Item) ((Item)o).Delete();
                        if (o is Mobile) ((Mobile)o).Delete();
                        Delete();
                    }
                }

                m_EndTime = DateTime.Now + Refractory;
            }
        }

        public override void OnDelete()
        {
            base.OnDelete();
            if (AttachedTo is Mobile)
            {
                Mobile m = (Mobile)AttachedTo;
                if (!m.Deleted)
                {
                    Effects.PlaySound(m, m.Map, 958);
                }
            }

            // Delete the minions
            foreach (BaseCreature b in MinionList)
            {
                if (b != null && !b.Deleted)
                    b.Delete();
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version 0
            writer.Write(m_Chance);
            writer.Write(m_Minion);
            writer.Write(m_Refractory);
            writer.Write(m_EndTime - DateTime.Now);
            writer.Write(MinionList.Count);
            foreach (BaseCreature b in MinionList)
                writer.Write(b);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            // version 0
            m_Chance = reader.ReadInt();
            m_Minion = reader.ReadString();
            Refractory = reader.ReadTimeSpan();
            TimeSpan remaining = reader.ReadTimeSpan();
            m_EndTime = DateTime.Now + remaining;
            int nminions = reader.ReadInt();
            for (int i = 0; i < nminions; i++)
            {
                BaseCreature b = (BaseCreature)reader.ReadMobile();
                MinionList.Add(b);
            }
        }
    }
}
