using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Spells;
using Server.Targeting;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a glacial eagle corpse")]
    public class GlacialEagle : BaseCreature
    {
        private DateTime m_NextFrostDive;
        private DateTime m_NextShatterScreech;
        private DateTime m_NextCrystalFeatherStorm;
        private bool m_AbilitiesInitialized;

        [Constructable]
        public GlacialEagle()
            : base(AIType.AI_Mage, FightMode.Aggressor, 10, 1, 0.2, 0.4)
        {
            Name = "a Glacial Eagle";
            Body = 5;
            Hue = 1150; // Icy blue hue
            BaseSoundID = 0x2EE;

            SetStr(450, 550);
            SetDex(200, 250);
            SetInt(350, 400);

            SetHits(500, 700);
            SetMana(300);

            SetDamage(20, 25);
            SetDamageType(ResistanceType.Cold, 70);
            SetDamageType(ResistanceType.Physical, 30);

            SetResistance(ResistanceType.Physical, 40, 50);
            SetResistance(ResistanceType.Fire, 10, 20);
            SetResistance(ResistanceType.Cold, 70, 90);
            SetResistance(ResistanceType.Poison, 20, 30);
            SetResistance(ResistanceType.Energy, 30, 40);

            SetSkill(SkillName.MagicResist, 80.0, 100.0);
            SetSkill(SkillName.Tactics, 80.0, 100.0);
            SetSkill(SkillName.Wrestling, 85.0, 95.0);
            SetSkill(SkillName.Magery, 90.0, 100.0);
            SetSkill(SkillName.EvalInt, 85.0, 100.0);

            Fame = 18000;
            Karma = -18000;

            VirtualArmor = 60;

            m_AbilitiesInitialized = false;
        }

        public GlacialEagle(Serial serial) : base(serial)
        {
        }

        public override bool CanFly => true;
        public override bool AutoDispel => true;
        public override int TreasureMapLevel => 4;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.FilthyRich, 2);
            AddLoot(LootPack.Gems, 5);

            if (Utility.RandomDouble() < 0.05) // 5% chance
                PackItem(new IceboundFeather()); // Custom loot item
        }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (!m_AbilitiesInitialized)
                {
                    m_NextFrostDive = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.Random(5, 15));
                    m_NextShatterScreech = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.Random(10, 20));
                    m_NextCrystalFeatherStorm = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.Random(15, 30));
                    m_AbilitiesInitialized = true;
                }

                if (DateTime.UtcNow >= m_NextFrostDive)
                    FrostDive();

                if (DateTime.UtcNow >= m_NextShatterScreech)
                    ShatterScreech();

                if (DateTime.UtcNow >= m_NextCrystalFeatherStorm)
                    CrystalFeatherStorm();
            }
        }

        private void FrostDive()
        {
            PublicOverheadMessage(MessageType.Regular, 0x480, true, "*The Glacial Eagle dives with a burst of frost!*");
            PlaySound(0x10B); // Impact sound

            if (Combatant != null && Combatant is Mobile target)
            {
                AOS.Damage(target, this, Utility.RandomMinMax(25, 40), 0, 0, 100, 0, 0);
                target.Freeze(TimeSpan.FromSeconds(2));
                target.SendMessage("A chilling dive stuns you momentarily!");

                Effects.SendLocationEffect(target.Location, target.Map, 0x3728, 30);
            }

            m_NextFrostDive = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.Random(20, 35));
        }

        private void ShatterScreech()
        {
            PublicOverheadMessage(MessageType.Regular, 0x481, true, "*A deafening screech shatters the air!*");
            PlaySound(0x229); // High-pitched scream

            foreach (Mobile m in GetMobilesInRange(4))
            {
                if (m != this && m.Alive && m is Mobile mob)
                {
                    mob.SendMessage("You are disoriented by the Glacial Eagle's screech!");
                    mob.Stam -= Utility.RandomMinMax(10, 20);
                    mob.Paralyzed = true;

                    Timer.DelayCall(TimeSpan.FromSeconds(2), () =>
                    {
                        if (mob != null && mob.Alive)
                            mob.Paralyzed = false;
                    });
                }
            }

            m_NextShatterScreech = DateTime.UtcNow + TimeSpan.FromSeconds(30);
        }

        private void CrystalFeatherStorm()
        {
            PublicOverheadMessage(MessageType.Regular, 0x480, true, "*The eagle flaps its wings, releasing razor-sharp feathers!*");
            PlaySound(0x34C); // Windy whoosh

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive && m is Mobile mob)
                {
                    int damage = Utility.RandomMinMax(10, 20);
                    AOS.Damage(mob, this, damage, 0, 0, 100, 0, 0);
                    mob.SendMessage("You are cut by flying crystal feathers!");
                }
            }

            Effects.SendLocationEffect(this.Location, this.Map, 0x372A, 25);

            m_NextCrystalFeatherStorm = DateTime.UtcNow + TimeSpan.FromSeconds(45);
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

    public class IceboundFeather : Item
    {
        [Constructable]
        public IceboundFeather() : base(0x1BD1)
        {
            Hue = 1150;
            Name = "an icebound feather";
            Weight = 0.1;
        }

        public IceboundFeather(Serial serial) : base(serial)
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
