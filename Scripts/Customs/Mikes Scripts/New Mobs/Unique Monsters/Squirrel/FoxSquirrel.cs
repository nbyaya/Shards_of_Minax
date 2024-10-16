using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a fox squirrel corpse")]
    public class FoxSquirrel : BaseCreature
    {
        private DateTime m_NextTailSwipe;
        private DateTime m_NextAcornBarrage;
        private DateTime m_NextAcornStorm;
        private bool m_AbilitiesInitialized;

        [Constructable]
        public FoxSquirrel()
            : base(AIType.AI_Melee, FightMode.Aggressor, 10, 1, 0.2, 0.4)
        {
            Name = "a Fox Squirrel";
            Body = 0x116; // Squirrel body
            Hue = 2433; // Bright orange and brown hue

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

        public FoxSquirrel(Serial serial)
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
                    m_NextTailSwipe = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextAcornBarrage = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextAcornStorm = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 40));
                    m_AbilitiesInitialized = true;
                }

                if (DateTime.UtcNow >= m_NextTailSwipe)
                {
                    TailSwipe();
                }

                if (DateTime.UtcNow >= m_NextAcornBarrage)
                {
                    AcornBarrage();
                }

                if (DateTime.UtcNow >= m_NextAcornStorm)
                {
                    AcornStorm();
                }
            }
        }

		private void TailSwipe()
		{
			PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Fox Squirrel sweeps its tail in a powerful arc! *");
			PlaySound(0x1F2); // Tail swipe sound effect

			Effects.SendLocationEffect(Location, Map, 0x3709, 10, 5); // Dust cloud effect

			foreach (Mobile m in GetMobilesInRange(2))
			{
				if (m != this && m.Alive && !m.IsDeadBondedPet)
				{
					AOS.Damage(m, this, Utility.RandomMinMax(15, 25), 0, 100, 0, 0, 0);
					m.SendMessage("You are hit by the Fox Squirrel's sweeping tail!");
					m.SendMessage("The dust cloud blinds you temporarily!");
					m.SendLocalizedMessage(1114817); // Temporary blindness
					Timer.DelayCall(TimeSpan.FromSeconds(0), () =>
					{
						// Implement a simple blindness effect using a timer
						m.SendMessage("You are momentarily blinded by the dust!");
						m.Freeze(TimeSpan.FromSeconds(5)); // Freeze the mob as a simple alternative
						Timer.DelayCall(TimeSpan.FromSeconds(5), () =>
						{
							m.SendMessage("The blindness wears off.");
						});
					});
				}
			}

			m_NextTailSwipe = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Cooldown for TailSwipe
		}


        private void AcornBarrage()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Fox Squirrel launches a barrage of acorns! *");
            PlaySound(0x1F3); // Acorn throw sound effect

            for (int i = 0; i < 5; i++)
            {
                Timer.DelayCall(TimeSpan.FromMilliseconds(i * 300), () =>
                {
                    if (Combatant != null)
                    {
                        Point3D loc = new Point3D(Location.X + Utility.Random(-2, 5), Location.Y + Utility.Random(-2, 5), Location.Z);
                        AcornItem acorn = new AcornItem();
                        acorn.MoveToWorld(loc, Map);

                        Timer.DelayCall(TimeSpan.FromSeconds(1), () => ExplodeAcorn(acorn));
                    }
                });
            }

            m_NextAcornBarrage = DateTime.UtcNow + TimeSpan.FromSeconds(45); // Cooldown for AcornBarrage
        }

        private void ExplodeAcorn(AcornItem acorn)
        {
            if (acorn.Deleted)
                return;

            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The acorn explodes on impact! *");
            PlaySound(0x1F4); // Acorn explosion sound effect

            Effects.SendLocationEffect(acorn.Location, Map, 0x36BD, 20, 10); // Explosion effect

            foreach (Mobile m in GetMobilesInRange(2))
            {
                if (m != this && m.Alive && !m.IsDeadBondedPet)
                {
                    AOS.Damage(m, this, Utility.RandomMinMax(10, 20), 0, 0, 100, 0, 0);
                    m.SendMessage("You are hit by the explosive acorn!");
                }
            }

            acorn.Delete();
        }

        private void AcornStorm()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Fox Squirrel calls forth a storm of acorns! *");
            PlaySound(0x1F5); // Storm sound effect

            for (int i = 0; i < 10; i++)
            {
                Timer.DelayCall(TimeSpan.FromMilliseconds(i * 200), () =>
                {
                    if (Combatant != null)
                    {
                        Point3D loc = new Point3D(Location.X + Utility.Random(-3, 6), Location.Y + Utility.Random(-3, 6), Location.Z);
                        AcornItem stormAcorn = new AcornItem();
                        stormAcorn.MoveToWorld(loc, Map);

                        Timer.DelayCall(TimeSpan.FromSeconds(1), () => ExplodeAcorn(stormAcorn));
                    }
                });
            }

            m_NextAcornStorm = DateTime.UtcNow + TimeSpan.FromSeconds(60); // Cooldown for AcornStorm
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

    public class AcornItem : Item
    {
        public AcornItem() : base(0x1726) // Acorn item ID
        {
            Movable = false;
        }

        public AcornItem(Serial serial) : base(serial)
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
