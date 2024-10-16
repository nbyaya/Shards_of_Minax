using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a venomous roe corpse")]
    public class VenomousRoe : BaseCreature
    {
        private DateTime m_NextToxicAura;
        private DateTime m_NextEnrage;
        private DateTime m_NextSummonMinions;

        private bool m_AbilitiesInitialized;

        [Constructable]
        public VenomousRoe()
            : base(AIType.AI_Animal, FightMode.Aggressor, 10, 1, 0.2, 0.4)
        {
            Name = "a venomous roe";
            Body = 0xEA; // GreatHart body
            Hue = 1970; // Toxic green hue

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

        public VenomousRoe(Serial serial)
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
            return 0x82; 
        }

        public override int GetHurtSound() 
        { 
            return 0x83; 
        }

        public override int GetDeathSound() 
        { 
            return 0x84; 
        }		

        public override void OnGaveMeleeAttack(Mobile defender)
        {
            base.OnGaveMeleeAttack(defender);

            if (0.25 > Utility.RandomDouble()) // 25% chance
            {
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Venomous Roe bites you with toxic fangs!*");
                defender.PlaySound(0x208);
                defender.FixedParticles(0x3709, 10, 30, 5052, EffectLayer.LeftFoot);

                int damage = Utility.RandomMinMax(5, 10);
                AOS.Damage(defender, this, damage, 0, 100, 0, 0, 0);

                defender.SendMessage("You feel a burning sensation as the venom takes hold!");
                Timer.DelayCall(TimeSpan.FromSeconds(1), new TimerCallback(delegate
                {
                    if (defender.Alive)
                    {
                        AOS.Damage(defender, this, Utility.RandomMinMax(2, 5), 0, 100, 0, 0, 0);
                    }
                }));
            }
        }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (!m_AbilitiesInitialized)
                {
                    Random rand = new Random();
                    m_NextToxicAura = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextEnrage = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 60));
                    m_NextSummonMinions = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 45));
                    m_AbilitiesInitialized = true;
                }

                if (DateTime.UtcNow >= m_NextToxicAura)
                {
                    ToxicAura();
                }

                if (DateTime.UtcNow >= m_NextEnrage)
                {
                    Enrage();
                }

                if (DateTime.UtcNow >= m_NextSummonMinions)
                {
                    SummonMinions();
                }
            }
        }

        private void ToxicAura()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Venomous Roe releases a toxic cloud!*");
            FixedEffect(0x36BD, 10, 16);

            foreach (Mobile m in GetMobilesInRange(3))
            {
                if (m != this && m.Player)
                {
                    m.SendMessage("The toxic cloud from the Venomous Roe weakens you!");
                    AOS.Damage(m, this, Utility.RandomMinMax(5, 10), 0, 0, 100, 0, 0);
                    m.Dex -= 10; // More significant debuff
                }
            }

            Random rand = new Random();
            m_NextToxicAura = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(20, 60)); // Random cooldown
        }

        private void Enrage()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Venomous Roe goes into a frenzied rage!*");
            PlaySound(0x208);
            FixedEffect(0x376A, 10, 16);

            SetDamage(15, 20); // Increase damage output
            SetResistance(ResistanceType.Physical, 50, 60); // Increase physical resistance

            Timer.DelayCall(TimeSpan.FromSeconds(10), new TimerCallback(ResetEnrage)); // Rage lasts for 10 seconds
            Random rand = new Random();
            m_NextEnrage = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(60, 120)); // Random cooldown
        }

        private void ResetEnrage()
        {
            SetDamage(10, 15); // Reset damage output
            SetResistance(ResistanceType.Physical, 30, 40); // Reset physical resistance
        }

        private void SummonMinions()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Venomous Roe summons toxic minions!*");
            PlaySound(0x21F);
            FixedEffect(0x376A, 10, 16);

            for (int i = 0; i < 2; i++) // Summon 2 minions
            {
                Point3D loc = GetSpawnPosition(2);

                if (loc != Point3D.Zero)
                {
                    ToxicWasp minion = new ToxicWasp();
                    minion.MoveToWorld(loc, Map);
                    minion.Combatant = Combatant;
                }
            }

            Random rand = new Random();
            m_NextSummonMinions = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(30, 90)); // Random cooldown
        }

        private Point3D GetSpawnPosition(int range)
        {
            for (int i = 0; i < 10; i++)
            {
                int x = X + Utility.RandomMinMax(-range, range);
                int y = Y + Utility.RandomMinMax(-range, range);
                int z = Map.GetAverageZ(x, y);

                Point3D p = new Point3D(x, y, z);

                if (Map.CanSpawnMobile(p))
                    return p;
            }

            return Point3D.Zero;
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

    public class ToxicWasp : BaseCreature
    {
        [Constructable]
        public ToxicWasp()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a toxic wasp";
            Body = 0x9D; // Wasp body
            Hue = 0x6F5; // Toxic green hue

            SetStr(30, 40);
            SetDex(60, 80);
            SetInt(20, 30);

            SetHits(20, 30);
            SetMana(0);

            SetDamage(4, 8);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 15, 25);
            SetResistance(ResistanceType.Cold, 5, 15);
            SetResistance(ResistanceType.Poison, 60, 70);

            SetSkill(SkillName.MagicResist, 20.0, 30.0);
            SetSkill(SkillName.Tactics, 30.0, 40.0);
            SetSkill(SkillName.Wrestling, 30.0, 40.0);

            Fame = 100;
            Karma = -100;

            VirtualArmor = 15;
        }

        public ToxicWasp(Serial serial)
            : base(serial)
        {
        }

        public override void OnGaveMeleeAttack(Mobile defender)
        {
            base.OnGaveMeleeAttack(defender);

            if (0.20 > Utility.RandomDouble()) // 20% chance
            {
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Toxic Wasp stings you with venom!*");
                defender.PlaySound(0x208);
                defender.FixedParticles(0x3709, 10, 30, 5052, EffectLayer.LeftFoot);

                int damage = Utility.RandomMinMax(2, 5);
                AOS.Damage(defender, this, damage, 0, 100, 0, 0, 0);

                defender.SendMessage("You feel a sharp pain as the venom begins to take effect!");
                Timer.DelayCall(TimeSpan.FromSeconds(1), new TimerCallback(delegate
                {
                    if (defender.Alive)
                    {
                        AOS.Damage(defender, this, Utility.RandomMinMax(1, 3), 0, 100, 0, 0, 0);
                    }
                }));
            }
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
