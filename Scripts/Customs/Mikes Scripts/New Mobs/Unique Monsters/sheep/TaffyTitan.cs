using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("a taffy titan corpse")]
    public class TaffyTitan : BaseCreature
    {
        private DateTime m_NextTaffyTwist;
        private DateTime m_NextStretchyShield;
        private DateTime m_NextTaffySlap;
        private DateTime m_NextTaffyBurst;
        private int m_StretchyShieldAbsorbed;
        private bool m_AbilitiesInitialized;

        [Constructable]
        public TaffyTitan()
            : base(AIType.AI_Melee, FightMode.Aggressor, 10, 1, 0.2, 0.4)
        {
            Name = "a Taffy Titan";
            Body = 0xCF; // Sheep body
            Hue = 2340; // Unique hue for Taffy Titan
			BaseSoundID = 0xD6;

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

            m_AbilitiesInitialized = false;
        }

        public TaffyTitan(Serial serial)
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
                    m_NextTaffyTwist = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextStretchyShield = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextTaffySlap = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 15));
                    m_NextTaffyBurst = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 25));
                    m_AbilitiesInitialized = true;
                }

                if (DateTime.UtcNow >= m_NextTaffyTwist)
                {
                    TaffyTwist();
                }

                if (DateTime.UtcNow >= m_NextStretchyShield)
                {
                    StretchyShield();
                }

                if (DateTime.UtcNow >= m_NextTaffySlap)
                {
                    TaffySlap();
                }

                if (DateTime.UtcNow >= m_NextTaffyBurst)
                {
                    TaffyBurst();
                }
            }

            if (m_StretchyShieldAbsorbed > 0 && DateTime.UtcNow >= m_NextStretchyShield)
            {
                DeactivateStretchyShield();
            }
        }

        private void TaffyTwist()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Taffy Titan ensnares its foes with a twisting whip of taffy!*");
            PlaySound(0x1A8); // Whip sound

            if (Combatant != null && CanBeHarmful(Combatant))
            {
                DoHarmful(Combatant);
                int damage = Utility.RandomMinMax(10, 20);
                AOS.Damage(Combatant, this, damage, 0, 100, 0, 0, 0);

                Mobile mob = Combatant as Mobile;
                if (mob != null)
                {
                    mob.Freeze(TimeSpan.FromSeconds(5));
                    Effects.SendLocationEffect(mob.Location, mob.Map, 0x373A, 20, 10); // Taffy effect
                    mob.SendMessage("You are ensnared by the twisting taffy!");
                }
            }

            m_NextTaffyTwist = DateTime.UtcNow + TimeSpan.FromSeconds(20);
        }

        private void StretchyShield()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Taffy Titan forms a stretchy shield!*");
            PlaySound(0x1E0); // Shield sound

            m_StretchyShieldAbsorbed = 40; // Absorbs up to 40 points of damage
            m_NextStretchyShield = DateTime.UtcNow + TimeSpan.FromSeconds(10);
            Effects.SendLocationEffect(this.Location, this.Map, 0x374A, 20, 10); // Shield effect
        }

        private void DeactivateStretchyShield()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Taffy Titan's stretchy shield fades away!*");
            m_StretchyShieldAbsorbed = 0;
        }

        private void TaffySlap()
        {
            if (Combatant != null && CanBeHarmful(Combatant))
            {
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Taffy Titan delivers a powerful taffy slap!*");
                PlaySound(0x3C9); // Slap sound

                int damage = Utility.RandomMinMax(15, 25);
                AOS.Damage(Combatant, this, damage, 0, 100, 0, 0, 0);

                Mobile mob = Combatant as Mobile;
                if (mob != null)
                {
                    mob.MovingParticles(this, 0x373A, 10, 0, false, true, 0x1F4, 0, 3006, 4006, 0x160, 0);
                    mob.SendMessage("You are hit by a powerful slap of taffy and are knocked back!");
                    mob.Direction = Direction.South; // Knockback effect
                }
            }

            m_NextTaffySlap = DateTime.UtcNow + TimeSpan.FromSeconds(15);
        }

        private void TaffyBurst()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Taffy Titan erupts in a burst of taffy!*");
            PlaySound(0x307); // Burst sound

            Effects.SendLocationEffect(this.Location, this.Map, 0x36BD, 20, 10); // Burst effect

            foreach (Mobile m in GetMobilesInRange(3))
            {
                if (m != this && m.Alive && CanBeHarmful(m))
                {
                    DoHarmful(m);
                    int damage = Utility.RandomMinMax(20, 30);
                    AOS.Damage(m, this, damage, 0, 100, 0, 0, 0);
                    m.SendMessage("You are caught in the taffy burst!");
                }
            }

            m_NextTaffyBurst = DateTime.UtcNow + TimeSpan.FromSeconds(30);
        }

        public override void AlterMeleeDamageFrom(Mobile from, ref int damage)
        {
            if (m_StretchyShieldAbsorbed > 0)
            {
                int absorbed = Math.Min(damage, m_StretchyShieldAbsorbed);
                damage -= absorbed;
                m_StretchyShieldAbsorbed -= absorbed;

                if (m_StretchyShieldAbsorbed == 0)
                {
                    DeactivateStretchyShield();
                }

                from.SendLocalizedMessage(1114728); // The shield absorbs the damage!
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(m_StretchyShieldAbsorbed);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_StretchyShieldAbsorbed = reader.ReadInt();
            m_AbilitiesInitialized = false;
        }
    }
}
