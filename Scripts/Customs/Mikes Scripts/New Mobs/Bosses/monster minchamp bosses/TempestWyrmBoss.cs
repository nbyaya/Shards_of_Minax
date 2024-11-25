using System;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a tempest wyrm boss corpse")]
    public class TempestWyrmBoss : TempestWyrm
    {
        private DateTime m_NextTempestBreath;
        private DateTime m_NextCycloneRampage;
        private bool m_IsRampaging;
        private bool m_AbilitiesActivated; // Flag for abilities activation

        [Constructable]
        public TempestWyrmBoss() : base()
        {
            Name = "Tempest Wyrm Overlord";
            Title = "the Stormbringer";
            Hue = 1157; // A more menacing stormy hue

            // Enhance stats to match or exceed Barracoon's or original boss tier values
            SetStr(1200); // Increased strength for boss tier
            SetDex(255); // Maximized dexterity for agility and combat effectiveness
            SetInt(250); // Increased intelligence for better skill usage

            SetHits(12000); // High health for boss fights
            SetDamage(40, 55); // Increased damage range for tougher encounters

            SetDamageType(ResistanceType.Physical, 60); 
            SetDamageType(ResistanceType.Fire, 25);
            SetDamageType(ResistanceType.Energy, 15);

            SetResistance(ResistanceType.Physical, 80); // Increased physical resistance
            SetResistance(ResistanceType.Fire, 70); // Increased fire resistance
            SetResistance(ResistanceType.Cold, 60); // Increased cold resistance
            SetResistance(ResistanceType.Poison, 100); // Poison resistance remains high
            SetResistance(ResistanceType.Energy, 60); // Increased energy resistance

            SetSkill(SkillName.Anatomy, 60.0, 100.0); 
            SetSkill(SkillName.EvalInt, 120.0, 150.0); 
            SetSkill(SkillName.Magery, 120.0, 150.0);
            SetSkill(SkillName.Meditation, 50.0, 75.0); 
            SetSkill(SkillName.MagicResist, 150.0, 180.0); 
            SetSkill(SkillName.Tactics, 120.0, 150.0);
            SetSkill(SkillName.Wrestling, 120.0, 150.0);

            Fame = 30000;
            Karma = -30000;

            VirtualArmor = 100;

            Tamable = false; // Prevent taming for the boss
            ControlSlots = 0; // Not tamable

            m_NextTempestBreath = DateTime.UtcNow;
            m_NextCycloneRampage = DateTime.UtcNow;
            m_AbilitiesActivated = false; // Initialize flag

            PackItem(new BossTreasureBox());
            XmlAttach.AttachTo(this, new XmlRandomAbility());
        }

        public override void GenerateLoot()
        {
            base.GenerateLoot();
            
            PackItem(new BossTreasureBox());
            for (int i = 0; i < 5; i++)
            {
                PackItem(new MaxxiaScroll());
            }

            // Optionally, you can add more loot as per your needs
            this.AddLoot(LootPack.FilthyRich, 2); // Rich loot for a boss-tier encounter
            this.AddLoot(LootPack.Rich);
            this.AddLoot(LootPack.Gems, 10);
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
                    m_NextTempestBreath = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 10));
                    m_NextCycloneRampage = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(5, 15));

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

        // Tempest Breath ability (modified for boss-tier)
        private void TempestBreath()
        {
            if (Combatant != null)
            {
                Mobile target = Combatant as Mobile;
                if (target != null && target.Alive)
                {
                    IPooledEnumerable nearby = GetMobilesInRange(7);
                    foreach (Mobile m in nearby)
                    {
                        if (m != this && m.Player && m.InLOS(this))
                        {
                            m.Damage(Utility.RandomMinMax(40, 60), this);
                            m.SendMessage("You are blasted by a powerful tempest breath!");
                            m.PlaySound(0x1F4); // Wind sound
                            m.BoltEffect(0); // Air effect
                            m.Location = new Point3D(m.X + Utility.RandomMinMax(-3, 3), m.Y + Utility.RandomMinMax(-3, 3), m.Z); // Knock back
                        }
                    }
                    nearby.Free();
                    m_NextTempestBreath = DateTime.UtcNow + TimeSpan.FromSeconds(25); // Reset timer for boss tier
                }
            }
        }

        // Cyclone Rampage ability (modified for boss-tier)
        private void CycloneRampage()
        {
            if (Combatant != null)
            {
                Mobile target = Combatant as Mobile;
                if (target != null && target.Alive)
                {
                    this.SendMessage("The tempest wyrm goes on a devastating rampage!");
                    this.Body = 12; // Indicate rampage with body change
                    this.VirtualArmor += 30; // Increased armor during rampage

                    // Increase damage and skills during rampage
                    this.SetDamage(35, 50);
                    this.SetSkill(SkillName.Tactics, 150.0);
                    this.SetSkill(SkillName.Wrestling, 150.0);

                    // Rampage lasts for 15 seconds
                    Timer.DelayCall(TimeSpan.FromSeconds(15), new TimerCallback(delegate()
                    {
                        this.Body = 13; // Revert back
                        this.VirtualArmor -= 30; // Revert armor
                        this.SetDamage(30, 45); // Revert damage
                        this.SetSkill(SkillName.Tactics, 120.0); // Revert tactics skill
                        this.SetSkill(SkillName.Wrestling, 120.0); // Revert wrestling skill
                    }));

                    m_IsRampaging = true;

                    IPooledEnumerable nearby = GetMobilesInRange(10);
                    foreach (Mobile m in nearby)
                    {
                        if (m != this && m.Player)
                        {
                            m.Damage(Utility.RandomMinMax(30, 50), this);
                            m.SendMessage("You are caught in the tempest's rampage!");
                            m.PlaySound(0x1F4); // Wind sound
                            m.BoltEffect(0); // Air effect
                        }
                    }
                    nearby.Free();

                    m_NextCycloneRampage = DateTime.UtcNow + TimeSpan.FromMinutes(2); // Reset timer
                    Timer.DelayCall(TimeSpan.FromSeconds(15), new TimerCallback(delegate() { m_IsRampaging = false; }));
                }
            }
        }

        public TempestWyrmBoss(Serial serial) : base(serial)
        {
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
            m_AbilitiesActivated = false; // Reset flag on deserialization
        }
    }
}
