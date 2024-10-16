using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Targeting;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("an albert's squirrel corpse")]
    public class AlbertsSquirrel : BaseCreature
    {
        private DateTime m_NextShadowBlend;
        private DateTime m_NextShadowStrike;
        private DateTime m_NextShadowClone;
        private DateTime m_NextDarkPulse;
        private bool m_AbilitiesInitialized;

        [Constructable]
        public AlbertsSquirrel()
            : base(AIType.AI_Animal, FightMode.Aggressor, 10, 1, 0.2, 0.4)
        {
            Name = "Albert's Squirrel";
            Body = 0x116; // Squirrel body
            Hue = 2441; // Dark brown with hints of red

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

            m_AbilitiesInitialized = false; // Initialize abilities flag
        }

        public AlbertsSquirrel(Serial serial)
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
        public override int GetAttackSound() 
        { 
            return 0xC9; 
        }

        public override int GetHurtSound() 
        { 
            return 0xCA; 
        }

        public override int GetDeathSound() 
        { 
            return 0xCB; 
        }
        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (!m_AbilitiesInitialized)
                {
                    Random rand = new Random();
                    m_NextShadowBlend = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 15));
                    m_NextShadowStrike = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextShadowClone = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 25));
                    m_NextDarkPulse = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_AbilitiesInitialized = true; // Set flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextShadowBlend)
                {
                    ShadowBlend();
                }

                if (DateTime.UtcNow >= m_NextShadowStrike)
                {
                    ShadowStrike();
                }

                if (DateTime.UtcNow >= m_NextShadowClone)
                {
                    ShadowClone();
                }

                if (DateTime.UtcNow >= m_NextDarkPulse)
                {
                    DarkPulse();
                }
            }
        }

        private void ShadowBlend()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Albert's Squirrel blends into the shadows, becoming nearly invisible! *");
            PlaySound(0x1D0); // Dark magic sound

            // Visual effect: dark mist
            FixedParticles(0x3709, 9, 32, 5006, EffectLayer.Waist);

            // Make the squirrel immune to attacks temporarily
            this.Hidden = true;
            Timer.DelayCall(TimeSpan.FromSeconds(3), () => this.Hidden = false);

            m_NextShadowBlend = DateTime.UtcNow + TimeSpan.FromSeconds(20); // Cooldown for ShadowBlend
        }

        private void ShadowStrike()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Albert's Squirrel strikes from the shadows with a burst of dark energy! *");
            PlaySound(0x1D0); // Dark magic sound

            Mobile target = Combatant as Mobile;
            if (target != null && !target.Hidden)
            {
                // Example condition: check if the target is in a specific direction (update logic if needed)
                int extraDamage = (target.X > this.X) ? Utility.RandomMinMax(15, 25) : Utility.RandomMinMax(5, 15);
                AOS.Damage(target, this, extraDamage, 0, 100, 0, 0, 0);

                // Stun effect
                target.SendMessage("You are stunned by the dark energy!");
                target.Frozen = true;
                Timer.DelayCall(TimeSpan.FromSeconds(2), () => target.Frozen = false);

                // Visual effect: burst of dark energy
                Effects.SendTargetEffect(target, 0x1F4, 16);
                target.SendMessage("You are struck by a burst of dark energy!");
            }

            m_NextShadowStrike = DateTime.UtcNow + TimeSpan.FromSeconds(25); // Cooldown for ShadowStrike
        }

        private void ShadowClone()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Albert's Squirrel summons a shadowy clone to confuse its foes! *");
            PlaySound(0x1D0); // Dark magic sound

            ShadowCloneItem clone = new ShadowCloneItem();
            clone.MoveToWorld(this.Location, this.Map);

            Timer.DelayCall(TimeSpan.FromSeconds(10), () => clone.Delete());

            m_NextShadowClone = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Cooldown for ShadowClone
        }

        private void DarkPulse()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Albert's Squirrel releases a wave of dark energy! *");
            PlaySound(0x1D0); // Dark magic sound

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive && !m.IsDeadBondedPet)
                {
                    int damage = Utility.RandomMinMax(10, 20);
                    AOS.Damage(m, this, damage, 0, 100, 0, 0, 0);
                    m.SendMessage("You are struck by a wave of dark energy!");
                    m.Freeze(TimeSpan.FromSeconds(1)); // Short stun effect
                    Effects.SendLocationEffect(m.Location, m.Map, 0x3709, 16, 4); // Dark mist effect
                }
            }

            m_NextDarkPulse = DateTime.UtcNow + TimeSpan.FromSeconds(40); // Cooldown for DarkPulse
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
            m_AbilitiesInitialized = false; // Reset abilities initialization flag
        }
    }

    public class ShadowCloneItem : Item
    {
        [Constructable]
        public ShadowCloneItem() : base(0x1D3) // Shadowy item ID
        {
            Movable = false;
            Name = "shadow clone";
        }

        public ShadowCloneItem(Serial serial) : base(serial)
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
        }
    }
}
