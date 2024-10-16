using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Network;
using Server.Spells;

namespace Server.Mobiles
{
    [CorpseName("a huntsman spider corpse")]
    public class HuntsmanSpider : GiantSpider
    {
        private DateTime m_NextRapidStrike;
        private DateTime m_NextQuickReflexes;
        private DateTime m_NextToxicVenom;
        private DateTime m_NextWebTrap;

        private bool m_AbilitiesInitialized;

        [Constructable]
        public HuntsmanSpider()
            : base()
        {
            Name = "a huntsman spider";
            Body = 28;
            Hue = 1790; // Unique hue for Huntsman Spider
			BaseSoundID = 0x388;

            SetStr(1000, 1200);
            SetDex(177, 255);
            SetInt(151, 250);
			
            SetHits(700, 1200);
			
            SetDamage(29, 35);
			
            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Fire, 25);
            SetDamageType(ResistanceType.Energy, 25);

            SetResistance(ResistanceType.Physical, 65, 80);
            SetResistance(ResistanceType.Fire, 60, 80);
            SetResistance(ResistanceType.Cold, 50, 60);
            SetResistance(ResistanceType.Poison, 65, 80);
            SetResistance(ResistanceType.Energy, 40, 50);

            SetSkill(SkillName.Anatomy, 25.1, 50.0);
            SetSkill(SkillName.EvalInt, 90.1, 100.0);
            SetSkill(SkillName.Magery, 95.5, 100.0);
            SetSkill(SkillName.Meditation, 25.1, 50.0);
            SetSkill(SkillName.MagicResist, 100.5, 150.0);
            SetSkill(SkillName.Tactics, 90.1, 100.0);
            SetSkill(SkillName.Wrestling, 90.1, 100.0);

            Fame = 24000;
            Karma = -24000;

            VirtualArmor = 90;
			
            Tamable = true;
            ControlSlots = 3;
            MinTameSkill = 93.9;

            m_AbilitiesInitialized = false; // Set flag to false initially
        }

        public HuntsmanSpider(Serial serial)
            : base(serial)
        {
        }

        public override bool ReacquireOnMovement => !Controlled;
        public override bool AutoDispel => !Controlled;
        public override int TreasureMapLevel => 5;
		public override bool CanAngerOnTame => true;
		public override void GenerateLoot()
		{
			this.AddLoot(LootPack.FilthyRich, 2);
			this.AddLoot(LootPack.Rich);
			this.AddLoot(LootPack.Gems, 8);
		}			

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (!m_AbilitiesInitialized)
                {
                    Random rand = new Random();
                    m_NextRapidStrike = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextQuickReflexes = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextToxicVenom = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 25));
                    m_NextWebTrap = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 35));
                    m_AbilitiesInitialized = true;
                }

                if (DateTime.UtcNow >= m_NextRapidStrike)
                {
                    RapidStrike();
                }

                if (DateTime.UtcNow >= m_NextQuickReflexes)
                {
                    QuickReflexes();
                }

                if (DateTime.UtcNow >= m_NextToxicVenom)
                {
                    ToxicVenom();
                }

                if (DateTime.UtcNow >= m_NextWebTrap)
                {
                    WebTrap();
                }
            }
        }

        private void RapidStrike()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Huntsman Spider strikes rapidly! *");
            for (int i = 0; i < 4; i++)
            {
                if (Combatant != null)
                {
                    this.DoHarmful(this.Combatant);
                    Timer.DelayCall(TimeSpan.FromSeconds(0.5), () => this.DoHarmful(this.Combatant));
                }
            }
            m_NextRapidStrike = DateTime.UtcNow + TimeSpan.FromSeconds(20);
        }

        private void QuickReflexes()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Huntsman Spider dodges gracefully! *");
            this.VirtualArmor += 15;
            Timer.DelayCall(TimeSpan.FromSeconds(10), () => this.VirtualArmor -= 15);
            m_NextQuickReflexes = DateTime.UtcNow + TimeSpan.FromSeconds(30);
        }

        private void ToxicVenom()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Huntsman Spider spits toxic venom! *");
            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Player && !m.IsDeadBondedPet)
                {
                    m.SendMessage("You are poisoned by the Huntsman Spider's venom!");
                    m.ApplyPoison(this, Poison.Lethal);
                }
            }
            m_NextToxicVenom = DateTime.UtcNow + TimeSpan.FromSeconds(25);
        }

        private void WebTrap()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Huntsman Spider spins a web trap! *");
            Point3D trapLocation = this.Location;
            Timer.DelayCall(TimeSpan.FromSeconds(1), () =>
            {
                if (Map.CanSpawnMobile(trapLocation))
                {
                    Item webTrap = new WebTrap(); // Corrected type here
                    webTrap.MoveToWorld(trapLocation, Map);
                }
            });
            m_NextWebTrap = DateTime.UtcNow + TimeSpan.FromSeconds(30);
        }

        public override void OnDamagedBySpell(Mobile caster)
        {
            base.OnDamagedBySpell(caster);

            if (0.25 > Utility.RandomDouble())
            {
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Huntsman Spider retaliates with a venomous bite! *");
                caster.SendMessage("The Huntsman Spider bites you with venom!");
                caster.ApplyPoison(this, Poison.Greater);
            }
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
            m_AbilitiesInitialized = false; // Reset initialization flag
        }
    }

    public class WebTrap : Item
    {
        [Constructable]
        public WebTrap()
            : base(0x1BEF) // Use an appropriate item ID
        {
            Movable = false;
            Name = "a trapdoor web";
        }

        private Timer m_Timer;
        private Timer m_DeleteTimer;

        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            if (m.InRange(this.GetWorldLocation(), 1))
            {
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* You are caught in a web trap! *");
                m.SendMessage("You are ensnared in a sticky web!");
                m.Freeze(TimeSpan.FromSeconds(5));
                this.Delete();
            }
        }

        private void StartDeleteTimer()
        {
            if (m_DeleteTimer != null)
                m_DeleteTimer.Stop();

            m_DeleteTimer = Timer.DelayCall(TimeSpan.FromSeconds(10), DeleteTimer); // Assuming you want it to delete after 10 seconds
        }

        private void DeleteTimer()
        {
            this.Delete();
        }

        public WebTrap(Serial serial)
            : base(serial)
        {
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
            StartDeleteTimer(); // Start the delete timer if needed
        }
    }
}
