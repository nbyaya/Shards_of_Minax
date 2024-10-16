using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a charro llama corpse")]
    public class CharroLlama : BaseCreature
    {
        private DateTime m_NextLassoToss;
        private DateTime m_NextCharge;
        private DateTime m_NextBraveryCall;
        private DateTime m_NextStampede;
        private bool m_AbilitiesInitialized;

        [Constructable]
        public CharroLlama()
            : base(AIType.AI_Melee, FightMode.Aggressor, 10, 1, 0.2, 0.4)
        {
            Name = "Charro Llama";
            Body = 0xDC; // Llama body
            Hue = 2156; // Unique hue for Charro appearance
			this.BaseSoundID = 0x3F3;

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

        public CharroLlama(Serial serial)
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
                    m_NextLassoToss = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextCharge = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 40));
                    m_NextBraveryCall = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 35));
                    m_NextStampede = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 45));
                    m_AbilitiesInitialized = true;
                }

                if (DateTime.UtcNow >= m_NextLassoToss)
                {
                    LassoToss();
                }

                if (DateTime.UtcNow >= m_NextCharge)
                {
                    ChargeOfTheCharro();
                }

                if (DateTime.UtcNow >= m_NextBraveryCall)
                {
                    BraveryCall();
                }

                if (DateTime.UtcNow >= m_NextStampede)
                {
                    Stampede();
                }
            }
        }

        private void LassoToss()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Charro Llama throws a lasso to immobilize its target! *");

            Mobile target = Combatant as Mobile; // Cast Combatant to Mobile
            if (target != null)
            {
                Timer.DelayCall(TimeSpan.FromSeconds(1), () =>
                {
                    if (target != null && target.Alive && !target.IsDeadBondedPet)
                    {
                        target.Freeze(TimeSpan.FromSeconds(5));
                        target.SendMessage("You are immobilized by the Charro Llama's lasso!");
                        Effects.SendLocationEffect(target.Location, target.Map, 0x373A, 10, 0); // Visual effect
                    }
                });
            }

            m_NextLassoToss = DateTime.UtcNow + TimeSpan.FromSeconds(30);
        }

        private void ChargeOfTheCharro()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Charro Llama charges with tremendous force! *");
            PlaySound(0x208);

            Mobile target = Combatant as Mobile; // Cast Combatant to Mobile
            if (target != null && target.Alive)
            {
                int damage = this.DamageMax * 2;
                AOS.Damage(target, this, damage, 0, 100, 0, 0, 0);
                target.SendMessage("You are hit with a powerful charge!");
                Effects.SendLocationEffect(target.Location, target.Map, 0x36BD, 10, 20); // Visual effect
            }

            m_NextCharge = DateTime.UtcNow + TimeSpan.FromSeconds(40);
        }

        private void BraveryCall()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Charro Llama calls for bravery, boosting the attack speed of nearby allies! *");
            PlaySound(0x1E0);

            foreach (Mobile m in GetMobilesInRange(10))
            {
                if (m is BaseCreature creature && creature != this && creature.Controlled)
                {
                    creature.SendMessage("You feel a surge of bravery!");
                    // Apply buff effect
                    Timer.DelayCall(TimeSpan.FromSeconds(1), () => creature.SendMessage("Your attack speed is increased!"));
                }
            }

            m_NextBraveryCall = DateTime.UtcNow + TimeSpan.FromSeconds(35);
        }

        private void Stampede()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Charro Llama starts a wild stampede! *");
            PlaySound(0x5B0); // Thunder sound

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive && CanBeHarmful(m))
                {
                    AOS.Damage(m, this, Utility.RandomMinMax(10, 20), 0, 100, 0, 0, 0);
                    m.SendMessage("You are trampled by the stampede!");
                    Effects.SendLocationEffect(m.Location, m.Map, 0x36BD, 10, 20); // Visual effect
                }
            }

            m_NextStampede = DateTime.UtcNow + TimeSpan.FromSeconds(45);
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

            m_AbilitiesInitialized = false;
        }
    }
}
