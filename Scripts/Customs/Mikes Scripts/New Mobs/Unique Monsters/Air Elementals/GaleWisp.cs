using System;
using Server.Items;
using Server.Network;
using Server.Mobiles;

namespace Server.Mobiles
{
    [CorpseName("a gale wisp corpse")]
    public class GaleWisp : BaseCreature
    {
        private DateTime m_NextWindBlast;
        private DateTime m_NextInvisibility;
        private DateTime m_NextGaleAura;
        private bool m_AbilitiesActivated; // New flag to track initial ability activation

        [Constructable]
        public GaleWisp()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a gale wisp";
            Body = 13; // Ghostly body, can be changed to a more fitting ID
            Hue = 1079; // Light blue hue
            BaseSoundID = 655; // Wind sound

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
            SetResistance(ResistanceType.Poison, 100);
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

            m_AbilitiesActivated = false; // Initialize flag
        }

        public GaleWisp(Serial serial)
            : base(serial)
        {
        }

		public override bool ReacquireOnMovement
		{
			get
			{
				return !Controlled;
			}
		}
		public override bool AutoDispel
		{
			get
			{
				return !Controlled;
			}
		}

		public override int TreasureMapLevel
		{
			get
			{
				return 5;
			}
		}
		
		public override bool CanAngerOnTame
		{
			get
			{
				return true;
			}
		}

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
                if (!m_AbilitiesActivated)
                {
                    // Randomly set the initial activation times
                    Random rand = new Random();
                    m_NextWindBlast = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextInvisibility = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextGaleAura = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 40));

                    m_AbilitiesActivated = true; // Set the flag to prevent re-initializing the times
                }

                if (DateTime.UtcNow >= m_NextWindBlast)
                {
                    WindBlast();
                }

                if (DateTime.UtcNow >= m_NextInvisibility)
                {
                    Invisibility();
                }

                if (DateTime.UtcNow >= m_NextGaleAura)
                {
                    GaleAura();
                }
            }
        }

        private void WindBlast()
        {
            if (Combatant != null)
            {
                Mobile target = Combatant as Mobile;
                if (target != null && target.Alive)
                {
                    // Deal area damage
                    foreach (Mobile m in GetMobilesInRange(3))
                    {
                        if (m != this && m.Alive)
                        {
                            int damage = Utility.RandomMinMax(10, 15);
                            m.Damage(damage, this);
                            m.SendMessage("You are hit by a powerful gust of wind!");

                            // Chance to knock back
                            int chance = Utility.Random(100);
                            if (chance < 30) // 30% chance to knock back
                            {
                                Effects.SendMovingEffect(m, this, 0x36D4, 16, 1, false, true);
                                m.Location = new Point3D(m.X + Utility.RandomMinMax(-2, 2), m.Y + Utility.RandomMinMax(-2, 2), m.Z);
                                m.SendMessage("You are knocked back by the gale!");
                            }
                        }
                    }
                    m_NextWindBlast = DateTime.UtcNow + TimeSpan.FromSeconds(30);
                }
            }
        }

        private void Invisibility()
        {
            if (!Hidden)
            {
                this.Hidden = true;
                this.PlaySound(0x1F5); // Sound of wind
                this.PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The gale wisp vanishes into thin air! *");
                Timer.DelayCall(TimeSpan.FromSeconds(10), new TimerCallback(delegate()
                {
                    if (Hidden)
                    {
                        this.Hidden = false;
                        this.PlaySound(0x1F5); // Sound of wind
                        this.PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The gale wisp reappears! *");
                    }
                }));
                m_NextInvisibility = DateTime.UtcNow + TimeSpan.FromMinutes(1);
            }
        }

        private void GaleAura()
        {
            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive)
                {
                    m.SendMessage("The strong wind around you slows your attacks!");
                    m.SendMessage("Your attack speed is reduced!");
                    m.Damage(0); // You can add effects to slow attack speed if needed
                }
            }
            m_NextGaleAura = DateTime.UtcNow + TimeSpan.FromMinutes(2);
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
