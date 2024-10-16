using System;
using Server.Items;
using Server.Network;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("a tempest wyrm corpse")]
    public class TempestWyrm : BaseCreature
    {
        private DateTime m_NextTempestBreath;
        private DateTime m_NextCycloneRampage;
        private bool m_IsRampaging;
        private bool m_AbilitiesActivated; // New flag to track initial ability activation

        [Constructable]
        public TempestWyrm()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a tempest wyrm";
            Body = 13; // Serpentine dragon-like body
            Hue = 1096; // Airy, stormy hue
            BaseSoundID = 362;

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

            m_NextTempestBreath = DateTime.UtcNow;
            m_NextCycloneRampage = DateTime.UtcNow;
            m_AbilitiesActivated = false; // Initialize flag

            ControlSlots = 4;
        }

        public TempestWyrm(Serial serial)
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
                    m_NextTempestBreath = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 15));
                    m_NextCycloneRampage = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));

                    m_AbilitiesActivated = true; // Set the flag to prevent re-initializing the times
                }

                if (DateTime.UtcNow >= m_NextTempestBreath)
                {
                    TempestBreath();
                }

                if (DateTime.UtcNow >= m_NextCycloneRampage && !m_IsRampaging)
                {
                    CycloneRampage();
                }
            }
        }

        private void TempestBreath()
        {
            if (Combatant != null)
            {
                Mobile target = Combatant as Mobile;
                if (target != null && target.Alive)
                {
                    IPooledEnumerable nearby = GetMobilesInRange(5);
                    foreach (Mobile m in nearby)
                    {
                        if (m != this && m.Player && m.InLOS(this))
                        {
                            m.Damage(Utility.RandomMinMax(20, 30), this);
                            m.SendMessage("You are blasted by a tempest breath!");
                            m.PlaySound(0x1F4); // Wind sound
                            m.BoltEffect(0); // Air effect
                            m.Location = new Point3D(m.X + Utility.RandomMinMax(-2, 2), m.Y + Utility.RandomMinMax(-2, 2), m.Z); // Knock back
                        }
                    }
                    nearby.Free();
                    m_NextTempestBreath = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Reset timer
                }
            }
        }

        private void CycloneRampage()
        {
            if (Combatant != null)
            {
                Mobile target = Combatant as Mobile;
                if (target != null && target.Alive)
                {
                    this.SendMessage("The tempest wyrm goes on a rampage!");
                    this.Body = 12; // Increase size to indicate rampage
                    this.VirtualArmor += 20; // Increase armor to indicate rampage

                    // Increase damage
                    this.SetDamage(25, 35);

                    // Rampage for 10 seconds
                    Timer.DelayCall(TimeSpan.FromSeconds(10), new TimerCallback(delegate()
                    {
                        this.Body = 13; // Revert size
                        this.VirtualArmor -= 20; // Revert armor
                        this.SetDamage(20, 30); // Revert damage
                    }));

                    m_IsRampaging = true;

                    IPooledEnumerable nearby = GetMobilesInRange(7);
                    foreach (Mobile m in nearby)
                    {
                        if (m != this && m.Player)
                        {
                            m.Damage(Utility.RandomMinMax(15, 25), this);
                            m.SendMessage("You are caught in the tempest's rampage!");
                            m.PlaySound(0x1F4); // Wind sound
                            m.BoltEffect(0); // Air effect
                        }
                    }
                    nearby.Free();

                    m_NextCycloneRampage = DateTime.UtcNow + TimeSpan.FromMinutes(1); // Reset timer
                    Timer.DelayCall(TimeSpan.FromSeconds(10), new TimerCallback(delegate() { m_IsRampaging = false; }));
                }
            }
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
            m_AbilitiesActivated = false; // Reset flag upon deserialization
        }
    }
}
