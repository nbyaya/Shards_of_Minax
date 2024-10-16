using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("an infinite pouncer corpse")]
    public class InfinitePouncer : BaseCreature
    {
        private DateTime m_NextEternalGaze;
        private DateTime m_NextMouthsOfMadness;
        private DateTime m_NextDimensionalRift;
        private bool m_AbilitiesInitialized;

        [Constructable]
        public InfinitePouncer()
            : base(AIType.AI_Melee, FightMode.Aggressor, 10, 1, 0.2, 0.4)
        {
            Name = "the Infinite Pouncer";
            Body = 205; // Rabbit body
            Hue = 2234; // Unique hue for a nightmarish appearance

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

        public InfinitePouncer(Serial serial)
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
                    m_NextEternalGaze = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextMouthsOfMadness = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextDimensionalRift = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 40));
                    m_AbilitiesInitialized = true;
                }

                if (DateTime.UtcNow >= m_NextEternalGaze)
                {
                    EternalGaze();
                }

                if (DateTime.UtcNow >= m_NextMouthsOfMadness)
                {
                    MouthsOfMadness();
                }

                if (DateTime.UtcNow >= m_NextDimensionalRift)
                {
                    DimensionalRift();
                }

                // Enrage Mechanism
                if (Hits < HitsMax * 0.3)
                {
                    Enrage();
                }
            }
        }

        private void EternalGaze()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Infinite Pouncer’s gaze unravels your sanity!*");
            PlaySound(0x212); // Unique sound for gaze

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive && CanBeHarmful(m))
                {
                    DoHarmful(m);

                    // Decrease stats and make them vulnerable
                    m.SendMessage("You feel your strength drain away as the gaze of the Infinite Pouncer seizes you!");
                    m.Dex -= 15;
                    m.Str -= 15;
                    m.Int -= 15;
                    m.VirtualArmor -= 15;
                    m.SendMessage("Your defenses weaken as you are cursed!");

                    m_NextEternalGaze = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Cooldown for EternalGaze
                }
            }
        }

        private void MouthsOfMadness()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Infinite Pouncer’s mouths attack in a frenzy!*");
            PlaySound(0x21A); // Unique sound for frenzy

            foreach (Mobile m in GetMobilesInRange(2))
            {
                if (m != this && m.Alive && CanBeHarmful(m))
                {
                    DoHarmful(m);
                    AOS.Damage(m, this, Utility.RandomMinMax(20, 30), 0, 0, 100, 0, 0);
                    m.SendMessage("You are overwhelmed by a frenzy of biting mouths!");
                }
            }

            m_NextMouthsOfMadness = DateTime.UtcNow + TimeSpan.FromSeconds(45); // Cooldown for MouthsOfMadness
        }

        private void DimensionalRift()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Infinite Pouncer tears open a dimensional rift!*");
            PlaySound(0x223); // Unique sound for rift opening

            Point3D loc = Location;
            DimensionalRiftItem rift = new DimensionalRiftItem();
            rift.MoveToWorld(loc, Map);

            Timer.DelayCall(TimeSpan.FromSeconds(2), () => ExplodeDimensionalRift(rift));

            m_NextDimensionalRift = DateTime.UtcNow + TimeSpan.FromSeconds(60); // Cooldown for DimensionalRift
        }

        private void ExplodeDimensionalRift(DimensionalRiftItem rift)
        {
            if (rift.Deleted)
                return;

            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The dimensional rift erupts with chaotic energies!*");
            PlaySound(0x225); // Unique sound for explosion

            Effects.SendLocationEffect(rift.Location, Map, 0x36BD, 20, 10); // Explosion effect

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive && !m.IsDeadBondedPet)
                {
                    int damage = Utility.RandomMinMax(25, 40);
                    AOS.Damage(m, this, damage, 0, 100, 0, 0, 0);
                    m.SendMessage("You are caught in the chaotic energies of the rift!");

                    // Apply random effects
                    switch (Utility.Random(3))
                    {
                        case 0: m.SendMessage("You feel a chill as ice forms around you!"); m.Freeze(TimeSpan.FromSeconds(5)); break;
                        case 1: m.SendMessage("You are engulfed in flames!"); m.ApplyPoison(this, Poison.Lethal); break;
                        case 2: m.SendMessage("You are struck by lightning!"); m.Damage(10); break;
                    }
                }
            }

            rift.Delete();
        }

        private void Enrage()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Infinite Pouncer becomes enraged!*");
            PlaySound(0x1B); // Roar sound

            SetDamage(35, 45);
            VirtualArmor = 100;

            // Increase speed and attack power
            SetDamage(35, 45);
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
            m_AbilitiesInitialized = false; // Reset flag on deserialize
        }
    }

    public class DimensionalRiftItem : Item
    {
        public DimensionalRiftItem() : base(0x171D)
        {
            Movable = false;
        }

        public DimensionalRiftItem(Serial serial) : base(serial)
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
        }
    }
}
