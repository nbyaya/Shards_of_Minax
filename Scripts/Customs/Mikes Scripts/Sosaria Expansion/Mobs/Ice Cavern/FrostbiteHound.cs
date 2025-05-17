using System;
using Server;
using Server.Mobiles;
using Server.Targeting;
using Server.Network;
using Server.Items;
using Server.Spells;

namespace Server.Mobiles
{
    [CorpseName("a frostbite hound corpse")]
    public class FrostbiteHound : BaseCreature
    {
        private DateTime m_NextFrostNova;
        private DateTime m_NextIceHowl;
        private DateTime m_NextFrozenVengeance;
        private bool m_AbilitiesInitialized;

        [Constructable]
        public FrostbiteHound()
            : base(AIType.AI_Melee, FightMode.Closest, 12, 1, 0.2, 0.4)
        {
            Name = "a Frostbite Hound";
            Body = 0xD9;
            Hue = 1150; // Pale icy blue unique hue
            BaseSoundID = 0x85;

            SetStr(450, 550);
            SetDex(200, 250);
            SetInt(100, 150);

            SetHits(500, 600);
            SetMana(0);

            SetDamage(20, 26);
            SetDamageType(ResistanceType.Cold, 80);
            SetDamageType(ResistanceType.Physical, 20);

            SetResistance(ResistanceType.Physical, 50, 60);
            SetResistance(ResistanceType.Fire, 10, 20);
            SetResistance(ResistanceType.Cold, 80, 95);
            SetResistance(ResistanceType.Poison, 30, 40);
            SetResistance(ResistanceType.Energy, 30, 40);

            SetSkill(SkillName.MagicResist, 80.0, 100.0);
            SetSkill(SkillName.Tactics, 85.0, 100.0);
            SetSkill(SkillName.Wrestling, 85.0, 100.0);

            Fame = 18000;
            Karma = -18000;

            VirtualArmor = 65;

            m_AbilitiesInitialized = false;
        }

        public FrostbiteHound(Serial serial)
            : base(serial)
        {
        }

        public override bool ReacquireOnMovement => !Controlled;
        public override bool AutoDispel => !Controlled;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Rich);
            AddLoot(LootPack.Gems, 2);

            if (Utility.RandomDouble() < 0.03) // 3% rare drop
                PackItem(new IceboundFang()); // Placeholder rare item
        }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null && Alive)
            {
                if (!m_AbilitiesInitialized)
                {
                    m_NextFrostNova = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(5, 15));
                    m_NextIceHowl = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 20));
                    m_NextFrozenVengeance = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
                    m_AbilitiesInitialized = true;
                }

                if (DateTime.UtcNow >= m_NextFrostNova)
                    CastFrostNova();

                if (DateTime.UtcNow >= m_NextIceHowl)
                    HowlOfIceboundTerror();

                if (DateTime.UtcNow >= m_NextFrozenVengeance)
                    FrozenVengeance();
            }
        }

        private void CastFrostNova()
        {
            PublicOverheadMessage(MessageType.Regular, 0x480, true, "* releases a pulse of freezing energy *");
            PlaySound(0x64C); // Chilling sound

            Effects.SendLocationEffect(Location, Map, 0x3709, 30, 10, 1150, 0);
            
            foreach (Mobile m in GetMobilesInRange(3))
            {
                if (m != this && m.Alive && !m.IsDeadBondedPet)
                {
                    m.Freeze(TimeSpan.FromSeconds(2));
                    AOS.Damage(m, this, Utility.RandomMinMax(15, 25), 0, 0, 100, 0, 0);

                    if (m is Mobile target)
                        target.SendMessage("The Frostbite Hound's icy nova freezes your limbs!");
                }
            }

            m_NextFrostNova = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 25));
        }

        private void HowlOfIceboundTerror()
        {
            PublicOverheadMessage(MessageType.Regular, 0x480, true, "* howls with bone-chilling fury *");
            PlaySound(0x6D6); // Ghostly howl sound

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive && !m.IsDeadBondedPet)
                {
                    if (Utility.RandomDouble() < 0.4)
                    {
                        m.Freeze(TimeSpan.FromSeconds(2));
                        if (m is Mobile mob)
                            mob.SendMessage("Terror grips your heart as the howl echoes through your soul!");
                    }
                }
            }

            m_NextIceHowl = DateTime.UtcNow + TimeSpan.FromSeconds(30);
        }

        private void FrozenVengeance()
        {
            if (Combatant != null)
            {
                PublicOverheadMessage(MessageType.Regular, 0x480, true, "* frost surges through its fangs! *");
                PlaySound(0x64C);

                AOS.Damage(Combatant, this, Utility.RandomMinMax(25, 40), 0, 0, 100, 0, 0);

                if (Combatant is Mobile target)
                {
                    target.Stam -= Utility.RandomMinMax(10, 20);
                    target.Mana -= Utility.RandomMinMax(10, 20);
                    target.SendMessage("You feel your strength and will drained by the cold bite!");
                }

                m_NextFrozenVengeance = DateTime.UtcNow + TimeSpan.FromSeconds(40);
            }
        }

        public override void OnDeath(Container c)
        {
            base.OnDeath(c);

            if (Utility.RandomDouble() < 0.05)
                c.DropItem(new ShardOfFrostQueen()); // A lore-tied rare drop
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
            writer.Write(m_AbilitiesInitialized);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_AbilitiesInitialized = reader.ReadBool();
        }
    }

    public class IceboundFang : Item
    {
        public IceboundFang() : base(0x1BD1)
        {
            Name = "Icebound Fang";
            Hue = 1150;
            LootType = LootType.Regular;
            Weight = 1.0;
        }

        public IceboundFang(Serial serial) : base(serial) { }

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

    public class ShardOfFrostQueen : Item
    {
        public ShardOfFrostQueen() : base(0x1EA7)
        {
            Name = "Shard of the Frost Queen";
            Hue = 1150;
            Weight = 0.5;
            LootType = LootType.Cursed;
        }

        public ShardOfFrostQueen(Serial serial) : base(serial) { }

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
