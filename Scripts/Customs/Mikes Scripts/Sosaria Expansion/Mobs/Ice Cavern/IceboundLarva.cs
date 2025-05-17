using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Spells;
using Server.Targeting;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("an icebound larva corpse")]
    public class IceboundLarva : BaseCreature
    {
        private DateTime m_NextCryoPulse;
        private DateTime m_NextShardBurst;
        private DateTime m_NextFrozenMind;
        private bool m_AbilitiesInitialized;

        [Constructable]
        public IceboundLarva()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "an Icebound Larva";
            Body = 778; // same as GazerLarva
            Hue = 1150; // icy blue hue
            BaseSoundID = 377;

            SetStr(200, 250);
            SetDex(60, 80);
            SetInt(200, 300);

            SetHits(400, 550);
            SetMana(300, 400);
            SetStam(80, 120);

            SetDamage(10, 18);
            SetDamageType(ResistanceType.Cold, 70);
            SetDamageType(ResistanceType.Physical, 30);

            SetResistance(ResistanceType.Physical, 40, 50);
            SetResistance(ResistanceType.Fire, 10, 20);
            SetResistance(ResistanceType.Cold, 70, 85);
            SetResistance(ResistanceType.Poison, 20, 30);
            SetResistance(ResistanceType.Energy, 40, 60);

            SetSkill(SkillName.Magery, 100.0, 120.0);
            SetSkill(SkillName.EvalInt, 90.0, 110.0);
            SetSkill(SkillName.MagicResist, 85.0, 105.0);
            SetSkill(SkillName.Tactics, 70.0, 90.0);
            SetSkill(SkillName.Wrestling, 60.0, 80.0);

            Fame = 15000;
            Karma = -15000;

            VirtualArmor = 50;
            m_AbilitiesInitialized = false;
        }

        public IceboundLarva(Serial serial)
            : base(serial)
        {
        }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (!m_AbilitiesInitialized)
                {
                    m_NextCryoPulse = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 20));
                    m_NextShardBurst = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 35));
                    m_NextFrozenMind = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(30, 45));
                    m_AbilitiesInitialized = true;
                }

                if (DateTime.UtcNow >= m_NextCryoPulse)
                    CryoPulse();

                if (DateTime.UtcNow >= m_NextShardBurst)
                    ShardBurst();

                if (DateTime.UtcNow >= m_NextFrozenMind)
                    FrozenMind();
            }
        }

        private void CryoPulse()
        {
            PublicOverheadMessage(MessageType.Regular, 0x480, true, "*The Icebound Larva pulses a wave of numbing cold!*");
            PlaySound(0x10B); // cold blast sound
            Effects.PlaySound(Location, Map, 0x10B);

            foreach (Mobile m in GetMobilesInRange(3))
            {
                if (m != this && m.Alive && !m.IsDeadBondedPet)
                {
                    AOS.Damage(m, this, Utility.RandomMinMax(10, 25), 0, 0, 100, 0, 0);
                    m.Freeze(TimeSpan.FromSeconds(2));
                    m.SendMessage(0x480, "Your limbs go numb from the sudden cold!");
                }
            }

            m_NextCryoPulse = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 40));
        }

        private void ShardBurst()
        {
            PublicOverheadMessage(MessageType.Regular, 0x480, true, "*The Icebound Larva erupts in a burst of razor-sharp ice shards!*");
            Effects.SendLocationEffect(Location, Map, 0x373A, 16, 1);
            PlaySound(0x1B3); // ice shatter

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive)
                {
                    int damage = Utility.RandomMinMax(20, 35);
                    AOS.Damage(m, this, damage, 0, 0, 100, 0, 0);

                    if (Utility.RandomDouble() < 0.2 && m is Mobile mobile)
                    {
                        mobile.Stam -= Utility.RandomMinMax(5, 15);
                        mobile.SendMessage(0x480, "Icy shards slow your movement!");
                    }
                }
            }

            m_NextShardBurst = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(45, 60));
        }

        private void FrozenMind()
        {
            if (Combatant is Mobile target)
            {
                PublicOverheadMessage(MessageType.Regular, 0x480, true, "*The Icebound Larva invades your mind with cold whispers...*");
                target.FixedParticles(0x373A, 10, 15, 5036, EffectLayer.Head);
                target.PlaySound(0x1E5);

                if (target.Mana > 0)
                    target.Mana -= Utility.RandomMinMax(10, 25);

                target.SendMessage(0x22, "Your thoughts slow under the weight of frost.");
            }

            m_NextFrozenMind = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(40, 70));
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Rich, 2);
            AddLoot(LootPack.Gems, 3);

            if (Utility.RandomDouble() < 0.02) // 2% chance
                PackItem(new FrostyCore());
        }

        public override int Meat => 1;

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            reader.ReadInt();
            m_AbilitiesInitialized = false;
        }
    }

    public class FrostyCore : Item
    {
        public FrostyCore() : base(0x1F18)
        {
            Hue = 1150;
            Name = "a frostbound core";
            Weight = 1.0;
            Movable = true;
        }

        public FrostyCore(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            reader.ReadInt();
        }
    }
}
