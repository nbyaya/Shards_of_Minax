using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("an Indian palm squirrel corpse")]
    public class IndianPalmSquirrel : BaseCreature
    {
        private DateTime m_NextPalmSlam;
        private DateTime m_NextQuickScurry;
        private DateTime m_NextTailSpin;
        private DateTime m_TailSpinEnd;
        private bool m_AbilitiesInitialized;

        [Constructable]
        public IndianPalmSquirrel()
            : base(AIType.AI_Animal, FightMode.Aggressor, 10, 1, 0.2, 0.4)
        {
            Name = "an Indian Palm Squirrel";
            Body = 0x116; // Squirrel body
            Hue = 2432; // Striped fur hue
            // Adjust the body and hue according to your server's specific definitions if necessary

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

        public IndianPalmSquirrel(Serial serial)
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
                    m_NextPalmSlam = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextQuickScurry = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 15));
                    m_NextTailSpin = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 25));
                    m_AbilitiesInitialized = true;
                }

                if (DateTime.UtcNow >= m_NextPalmSlam)
                {
                    PalmSlam();
                }

                if (DateTime.UtcNow >= m_NextQuickScurry)
                {
                    QuickScurry();
                }

                if (DateTime.UtcNow >= m_NextTailSpin)
                {
                    TailSpin();
                }

                if (DateTime.UtcNow >= m_TailSpinEnd && m_TailSpinEnd != DateTime.MinValue)
                {
                    EndTailSpin();
                }
            }
        }

        private void PalmSlam()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Indian Palm Squirrel slams its tail into the ground, causing a shockwave! *");
            PlaySound(0x1FE); // Sound for shockwave

            // Create shockwave effect
            Effects.SendLocationEffect(Location, Map, 0x36BD, 20, 10); // Shockwave effect
            foreach (Mobile m in GetMobilesInRange(3))
            {
                if (m != this && m.Alive && CanBeHarmful(m))
                {
                    DoHarmful(m);
                    AOS.Damage(m, this, Utility.RandomMinMax(15, 25), 0, 100, 0, 0, 0); // Shockwave damage
                    m.SendMessage("You are knocked down by the shockwave!");
                    m.Freeze(TimeSpan.FromSeconds(2)); // Stun effect
                }
            }

            m_NextPalmSlam = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Cooldown for Palm Slam
        }

        private void QuickScurry()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Indian Palm Squirrel quickly scurries around, evading attacks! *");
            PlaySound(0x1F4); // Sound for scurrying

            // Blur effect for Quick Scurry
            MovingParticles(this, 0x373A, 10, 0, false, true, 0x1F4, 0, 3006, 4006, 0x160, 0);

            // Evade by teleporting to a random location within 5 tiles
            Point3D newLocation = new Point3D(
                Location.X + Utility.RandomMinMax(-5, 5),
                Location.Y + Utility.RandomMinMax(-5, 5),
                Location.Z
            );

            if (Map.CanFit(newLocation, 16, false, false))
            {
                Location = newLocation;
            }

            m_NextQuickScurry = DateTime.UtcNow + TimeSpan.FromSeconds(20); // Cooldown for Quick Scurry
        }

        private void TailSpin()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Indian Palm Squirrel spins rapidly, creating a whirlwind of motion! *");
            PlaySound(0x1F5); // Sound for tail spin

            // Tail spin effect
            Effects.SendLocationEffect(Location, Map, 0x376A, 20, 10); // Wind effect

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive && CanBeHarmful(m))
                {
                    DoHarmful(m);
                    AOS.Damage(m, this, Utility.RandomMinMax(10, 15), 0, 100, 0, 0, 0); // Wind damage
                    m.SendMessage("You are caught in the whirlwind created by the squirrel's tail spin!");
                }
            }

            m_TailSpinEnd = DateTime.UtcNow + TimeSpan.FromSeconds(10); // Duration of Tail Spin
            m_NextTailSpin = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Cooldown for Tail Spin
        }

        private void EndTailSpin()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The whirlwind created by the squirrel's tail spin dissipates. *");
            // You can add more effects here if desired
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

            m_AbilitiesInitialized = false; // Reset flag on deserialize
        }
    }
}
