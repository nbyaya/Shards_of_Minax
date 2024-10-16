using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("a steam leviathan corpse")]
    public class SteamLeviathan : BaseCreature
    {
        private DateTime m_NextSteamBlast;
        private DateTime m_NextPressureWave;
        private DateTime m_NextBoilingSurge;

        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public SteamLeviathan()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a steam leviathan";
            Body = 16; // Water elemental body
            Hue = 2502; // Steam or vapor-like color
            BaseSoundID = 278;

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

            m_AbilitiesInitialized = false; // Initialize flag
        }

        public SteamLeviathan(Serial serial)
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
                    m_NextSteamBlast = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextPressureWave = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextBoilingSurge = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 40));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextSteamBlast)
                {
                    SteamBlast();
                }

                if (DateTime.UtcNow >= m_NextPressureWave)
                {
                    PressureWave();
                }

                if (DateTime.UtcNow >= m_NextBoilingSurge)
                {
                    BoilingSurge();
                }
            }
        }

        private void SteamBlast()
        {
            if (Combatant is Mobile target && target.Alive)
            {
                int damage = Utility.RandomMinMax(20, 30);
                target.Damage(damage, this);
                target.SendMessage("You are scalded by the Steam Leviathan's blast of steam!");
                Effects.SendTargetEffect(target, 0x36D4, 16); // Steam effect
                m_NextSteamBlast = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Cooldown for SteamBlast
            }
        }

        private void PressureWave()
        {
            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m != Combatant && m.Alive)
                {
                    int damage = Utility.RandomMinMax(15, 20);
                    m.Damage(damage, this);
                    m.SendMessage("A shockwave from the Steam Leviathan disorients you!");
                    m.Freeze(TimeSpan.FromSeconds(2)); // Disorient effect
                }
            }

            Effects.SendLocationParticles(EffectItem.Create(this.Location, this.Map, EffectItem.DefaultDuration), 0x36D4, 10, 30, 9917);
            m_NextPressureWave = DateTime.UtcNow + TimeSpan.FromSeconds(40); // Cooldown for PressureWave
        }

        private void BoilingSurge()
        {
            this.SetDamage(this.DamageMin + 10, this.DamageMax + 10);
            this.SendMessage("The Steam Leviathan's boiling aura intensifies its power!");
            Effects.SendLocationParticles(EffectItem.Create(this.Location, this.Map, EffectItem.DefaultDuration), 0x36D4, 10, 30, 9917);
            Timer.DelayCall(TimeSpan.FromSeconds(15), new TimerCallback(() =>
            {
                this.SetDamage(this.DamageMin - 10, this.DamageMax - 10);
            }));
            m_NextBoilingSurge = DateTime.UtcNow + TimeSpan.FromMinutes(2); // Cooldown for BoilingSurge
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
            
            m_AbilitiesInitialized = false; // Reset the initialization flag on deserialize
        }
    }
}
