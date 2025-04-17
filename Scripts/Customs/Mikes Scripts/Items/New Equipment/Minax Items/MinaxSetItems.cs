using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    //===========================
    // Minax Set Bonus Manager
    //===========================
    public static class MinaxSetBonusManager
    {
        // Store active bonus timers keyed by Mobile
        private static Dictionary<Mobile, Timer> m_ActiveTimers = new Dictionary<Mobile, Timer>();

        // Bonus values
        private const int BonusFollowers = 5;
        private static readonly TimeSpan SummonInterval = TimeSpan.FromSeconds(15.0);

        // Checks if the full Minax set is equipped by the mobile
        public static bool HasFullSet(Mobile m)
        {
            if (m == null)
                return false;

            // Check each layer for the specific Minax set item type
            return (m.FindItemOnLayer(Layer.InnerTorso) is MinaxFemaleStuddedChest) &&
                   (m.FindItemOnLayer(Layer.Arms) is MinaxLeatherArms) &&
                   (m.FindItemOnLayer(Layer.Gloves) is MinaxLeatherGloves) &&
                   (m.FindItemOnLayer(Layer.Helm) is MinaxStandardPlateKabuto) &&
                   (m.FindItemOnLayer(Layer.Shoes) is MinaxThighBoots) &&
                   (m.FindItemOnLayer(Layer.Cloak) is MinaxCloak);
        }

        // Update the bonus status on the mobile
        public static void UpdateBonus(Mobile m)
        {
            if (m == null || !(m is PlayerMobile))
                return;

            if (HasFullSet(m))
            {
                // Activate bonus if not already active
                if (!m_ActiveTimers.ContainsKey(m))
                {
                    m.FollowersMax += BonusFollowers;
                    m.SendMessage(78, "The Minax set empowers you with greater command over creatures!");

                    Timer timer = new MinaxSetBonusTimer(m, SummonInterval);
                    m_ActiveTimers[m] = timer;
                    timer.Start();
                }
            }
            else
            {
                // Remove bonus if active
                if (m_ActiveTimers.ContainsKey(m))
                {
                    m.FollowersMax -= BonusFollowers;
                    m.SendMessage(37, "The full power of the Minax set has faded.");

                    m_ActiveTimers[m].Stop();
                    m_ActiveTimers.Remove(m);
                }
            }
        }

        // Timer that summons a Time Demon every interval if there's room for more followers
        private class MinaxSetBonusTimer : Timer
        {
            private Mobile m_Owner;

            public MinaxSetBonusTimer(Mobile owner, TimeSpan interval)
                : base(interval, interval)
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                // If owner is deleted or not wearing the full set, stop the timer
                if (m_Owner == null || m_Owner.Deleted || !HasFullSet(m_Owner))
                {
                    Stop();
                    m_ActiveTimers.Remove(m_Owner);
                    return;
                }

                // Check autosummon status (if applicable)
                if (!AutoSummonManager.IsAutoSummonEnabled(m_Owner))
                    return;

                // Only summon if there is room for more followers
                if (m_Owner.Followers < m_Owner.FollowersMax)
                {
                    TimeDemon demon = new TimeDemon
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    demon.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "A Time Demon is drawn forth by the power of the Minax set!");
                }
            }
        }
    }

    //===========================
    // Minax Set Pieces
    //===========================

    // 1. FemaleStuddedChest piece
    public class MinaxFemaleStuddedChest : FemaleStuddedChest
    {
        [Constructable]
        public MinaxFemaleStuddedChest()
        {
            Name = "Minax Chest";
            Hue = Utility.RandomMinMax(1, 2999);
            // (Optional: add any unique attributes or bonuses here)
            XmlAttach.AttachTo(this, new XmlLevelItem());
        }

        public MinaxFemaleStuddedChest(Serial serial) : base(serial) { }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);
            if (parent is PlayerMobile pm)
                MinaxSetBonusManager.UpdateBonus(pm);
        }

        public override void OnRemoved(object parent)
        {
            base.OnRemoved(parent);
            if (parent is PlayerMobile pm)
                MinaxSetBonusManager.UpdateBonus(pm);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    // 2. LeatherArms piece
    public class MinaxLeatherArms : LeatherArms
    {
        [Constructable]
        public MinaxLeatherArms()
        {
            Name = "Minax Arms";
            Hue = Utility.RandomMinMax(1, 2999);
            XmlAttach.AttachTo(this, new XmlLevelItem());
        }

        public MinaxLeatherArms(Serial serial) : base(serial) { }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);
            if (parent is PlayerMobile pm)
                MinaxSetBonusManager.UpdateBonus(pm);
        }

        public override void OnRemoved(object parent)
        {
            base.OnRemoved(parent);
            if (parent is PlayerMobile pm)
                MinaxSetBonusManager.UpdateBonus(pm);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    // 3. LeatherGloves piece
    public class MinaxLeatherGloves : LeatherGloves
    {
        [Constructable]
        public MinaxLeatherGloves()
        {
            Name = "Minax Gloves";
            Hue = Utility.RandomMinMax(1, 2999);
            XmlAttach.AttachTo(this, new XmlLevelItem());
        }

        public MinaxLeatherGloves(Serial serial) : base(serial) { }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);
            if (parent is PlayerMobile pm)
                MinaxSetBonusManager.UpdateBonus(pm);
        }

        public override void OnRemoved(object parent)
        {
            base.OnRemoved(parent);
            if (parent is PlayerMobile pm)
                MinaxSetBonusManager.UpdateBonus(pm);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    // 4. StandardPlateKabuto piece (helmet)
    public class MinaxStandardPlateKabuto : DragonHelm
    {
        [Constructable]
        public MinaxStandardPlateKabuto()
        {
            Name = "Minax Helm";
            Hue = Utility.RandomMinMax(1, 2999);
            // You may include attributes similar to a Chrono Crown if desired
            XmlAttach.AttachTo(this, new XmlLevelItem());
        }

        public MinaxStandardPlateKabuto(Serial serial) : base(serial) { }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);
            if (parent is PlayerMobile pm)
                MinaxSetBonusManager.UpdateBonus(pm);
        }

        public override void OnRemoved(object parent)
        {
            base.OnRemoved(parent);
            if (parent is PlayerMobile pm)
                MinaxSetBonusManager.UpdateBonus(pm);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    // 5. ThighBoots piece
    public class MinaxThighBoots : ThighBoots
    {
        [Constructable]
        public MinaxThighBoots()
        {
            Name = "Minax Boots";
            Hue = Utility.RandomMinMax(1, 2999);
            XmlAttach.AttachTo(this, new XmlLevelItem());
        }

        public MinaxThighBoots(Serial serial) : base(serial) { }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);
            if (parent is PlayerMobile pm)
                MinaxSetBonusManager.UpdateBonus(pm);
        }

        public override void OnRemoved(object parent)
        {
            base.OnRemoved(parent);
            if (parent is PlayerMobile pm)
                MinaxSetBonusManager.UpdateBonus(pm);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    // 6. Cloak piece
    public class MinaxCloak : Cloak
    {
        [Constructable]
        public MinaxCloak()
        {
            Name = "Minax Cloak";
            Hue = Utility.RandomMinMax(1, 2999);
            XmlAttach.AttachTo(this, new XmlLevelItem());
        }

        public MinaxCloak(Serial serial) : base(serial) { }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);
            if (parent is PlayerMobile pm)
                MinaxSetBonusManager.UpdateBonus(pm);
        }

        public override void OnRemoved(object parent)
        {
            base.OnRemoved(parent);
            if (parent is PlayerMobile pm)
                MinaxSetBonusManager.UpdateBonus(pm);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
