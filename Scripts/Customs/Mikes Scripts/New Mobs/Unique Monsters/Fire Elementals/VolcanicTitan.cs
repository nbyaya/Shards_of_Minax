using System;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a volcanic titan corpse")]
    public class VolcanicTitan : BaseCreature
    {
        private DateTime m_NextEruption;
        private DateTime m_NextMagmaWave;
        private DateTime m_NextFury;
        private bool m_AbilitiesInitialized;
        private bool m_InFury;

        [Constructable]
        public VolcanicTitan()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a volcanic titan";
            Body = 15; // Fire elemental body
            Hue = 1592; // Unique hue for volcanic appearance
            BaseSoundID = 838;

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

            this.PackItem(new SulfurousAsh(5));

            m_AbilitiesInitialized = false; // Initialize flag
            m_InFury = false;
        }

        public VolcanicTitan(Serial serial)
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
                    // Set random intervals for abilities
                    Random rand = new Random();
                    m_NextEruption = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 60)); // Random time between 10 to 60 seconds
                    m_NextMagmaWave = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 90)); // Random time between 15 to 90 seconds
                    m_NextFury = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 120)); // Random time between 20 to 120 seconds
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextEruption)
                {
                    VolcanicEruption();
                }

                if (DateTime.UtcNow >= m_NextMagmaWave)
                {
                    MagmaWave();
                }

                if (DateTime.UtcNow >= m_NextFury)
                {
                    TitanFury();
                }
            }
        }

        private void VolcanicEruption()
        {
            if (Combatant != null)
            {
                Mobile target = Combatant as Mobile;
                if (target != null && target.Alive)
                {
                    foreach (Mobile m in GetMobilesInRange(5))
                    {
                        if (m != this && m.Alive)
                        {
                            int damage = Utility.RandomMinMax(30, 50);
                            m.Damage(damage, this); // Apply fire damage
                            m.SendMessage("The ground erupts in a fiery explosion!");
                        }
                    }
                }

                m_NextEruption = DateTime.UtcNow + TimeSpan.FromMinutes(2); // Reset cooldown after use
            }
        }

        private void MagmaWave()
        {
            if (Combatant != null)
            {
                Mobile target = Combatant as Mobile;
                if (target != null && target.Alive)
                {
                    foreach (Mobile m in GetMobilesInRange(5))
                    {
                        if (m != this && m.Alive)
                        {
                            int damage = Utility.RandomMinMax(20, 35);
                            m.Damage(damage, this); // Apply fire damage
                            m.SendMessage("A wave of molten lava burns you!");
                        }
                    }
                }

                m_NextMagmaWave = DateTime.UtcNow + TimeSpan.FromMinutes(1); // Reset cooldown after use
            }
        }

        private void TitanFury()
        {
            if (!m_InFury)
            {
                m_InFury = true;
                this.SetStr(this.Str + 50);
                this.SetDex(this.Dex + 50);
                this.SetDamage(this.DamageMin + 10, this.DamageMax + 10);
                this.SendMessage("The titan's fury surges, increasing its strength and speed!");

                Timer.DelayCall(TimeSpan.FromSeconds(20), () =>
                {
                    this.SetStr(this.Str - 50);
                    this.SetDex(this.Dex - 50);
                    this.SetDamage(this.DamageMin - 10, this.DamageMax - 10);
                    m_InFury = false;
                    this.SendMessage("The titan's fury subsides.");
                });
            }

            m_NextFury = DateTime.UtcNow + TimeSpan.FromMinutes(1); // Reset cooldown after use
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

            m_AbilitiesInitialized = false; // Reset initialization flag on deserialize
        }
    }
}
