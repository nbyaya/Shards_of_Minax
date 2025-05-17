using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Targeting;
using Server.Network;
using Server.Spells;

namespace Server.Mobiles
{
    [CorpseName("a frostbitten steed corpse")]
    public class FrostbittenSteed : BaseMount, IElementalCreature
    {
        private DateTime m_NextFrostNova;
        private DateTime m_NextIcyCharge;
        private DateTime m_NextShatterCry;

        public ElementType ElementType { get { return ElementType.Cold; } }

        [Constructable]
        public FrostbittenSteed() : this("a frostbitten steed")
        {
        }

        [Constructable]
        public FrostbittenSteed(string name)
            : base(name, 793, 0x3EBB, AIType.AI_Melee, FightMode.Aggressor, 12, 1, 0.2, 0.4)
        {
            Hue = 1150; // A unique cold bluish hue
            BaseSoundID = 0xA8; // Icy shriek

            SetStr(300, 350);
            SetDex(160, 200);
            SetInt(150, 180);

            SetHits(800, 1000);
            SetMana(400, 500);

            SetDamage(22, 28);

            SetDamageType(ResistanceType.Cold, 80);
            SetDamageType(ResistanceType.Physical, 20);

            SetResistance(ResistanceType.Physical, 65, 80);
            SetResistance(ResistanceType.Cold, 90, 100);
            SetResistance(ResistanceType.Fire, 10, 20);
            SetResistance(ResistanceType.Poison, 50, 60);
            SetResistance(ResistanceType.Energy, 40, 50);

            SetSkill(SkillName.MagicResist, 100.0, 120.0);
            SetSkill(SkillName.Tactics, 95.0, 110.0);
            SetSkill(SkillName.Wrestling, 95.0, 115.0);
            SetSkill(SkillName.Magery, 70.0, 90.0);
            SetSkill(SkillName.EvalInt, 70.0, 90.0);

            Fame = 24000;
            Karma = -24000;

            Tamable = true;

            ControlSlots = 5;
            MinTameSkill = 115.0;

            m_NextFrostNova = DateTime.UtcNow + TimeSpan.FromSeconds(15);
            m_NextIcyCharge = DateTime.UtcNow + TimeSpan.FromSeconds(25);
            m_NextShatterCry = DateTime.UtcNow + TimeSpan.FromSeconds(35);
        }

        public FrostbittenSteed(Serial serial)
            : base(serial)
        {
        }

        public override Poison PoisonImmune => Poison.Lethal;

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (DateTime.UtcNow >= m_NextFrostNova)
                    FrostNova();

                if (DateTime.UtcNow >= m_NextIcyCharge)
                    IcyCharge();

                if (DateTime.UtcNow >= m_NextShatterCry)
                    ShatterCry();
            }
        }

        private void FrostNova()
        {
            PublicOverheadMessage(MessageType.Emote, 0x480, true, "*releases a devastating Frost Nova*");
            PlaySound(0x10B); // Frost sound

            Effects.SendLocationEffect(Location, Map, 0x37BE, 16, 10); // Cold pulse effect

            foreach (Mobile m in GetMobilesInRange(3))
            {
                if (m != this && m.Alive && !m.IsDeadBondedPet)
                {
                    AOS.Damage(m, this, Utility.RandomMinMax(20, 35), 0, 0, 100, 0, 0);
                    m.Freeze(TimeSpan.FromSeconds(2));
                    if (m is Mobile mobile)
                        mobile.SendMessage(0x480, "The frost chills you to the bone!");
                }
            }

            m_NextFrostNova = DateTime.UtcNow + TimeSpan.FromSeconds(30);
        }

        private void IcyCharge()
        {
            if (Combatant is Mobile target)
            {
                PublicOverheadMessage(MessageType.Regular, 0x480, true, "*charges forward with glacial fury!*");
                Effects.SendMovingEffect(this, target, 0x3818, 5, 0, false, false, Hue, 0);
                Timer.DelayCall(TimeSpan.FromSeconds(0.5), () =>
                {
                    if (!Deleted && target.Alive)
                    {
                        AOS.Damage(target, this, Utility.RandomMinMax(25, 40), 0, 0, 100, 0, 0);
                        target.SendMessage(0x480, "You're knocked back by the icy assault!");
                        target.Freeze(TimeSpan.FromSeconds(1));
                    }
                });

                m_NextIcyCharge = DateTime.UtcNow + TimeSpan.FromSeconds(25);
            }
        }

        private void ShatterCry()
        {
            PublicOverheadMessage(MessageType.Emote, 0x480, true, "*lets out a shattering cry that fractures ice and minds alike*");
            PlaySound(0x64F); // High-pitched scream

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive && m is Mobile target)
                {
                    if (Utility.RandomDouble() < 0.3)
                    {
                        target.SendMessage(0x22, "Your vision blurs with frost!");
                        target.Paralyze(TimeSpan.FromSeconds(2));
                        target.Mana -= 20;
                        target.Stam -= 20;
                    }
                }
            }

            m_NextShatterCry = DateTime.UtcNow + TimeSpan.FromSeconds(45);
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich);
            AddLoot(LootPack.Gems, 5);

            if (Utility.RandomDouble() < 0.01)
                PackItem(new IceboundHeart()); // Rare drop
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

            m_NextFrostNova = DateTime.UtcNow + TimeSpan.FromSeconds(15);
            m_NextIcyCharge = DateTime.UtcNow + TimeSpan.FromSeconds(25);
            m_NextShatterCry = DateTime.UtcNow + TimeSpan.FromSeconds(35);
        }
    }

    public class IceboundHeart : Item
    {
        [Constructable]
        public IceboundHeart() : base(0x1CF0) // Blue gem ID
        {
            Name = "Icebound Heart";
            Hue = 1150;
            Weight = 1.0;
        }

        public IceboundHeart(Serial serial) : base(serial) { }

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
