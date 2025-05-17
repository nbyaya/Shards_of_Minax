using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Network;
using Server.Targeting;
using Server.Spells;

namespace Server.Mobiles
{
    [CorpseName("the corpse of the Serpent of the Glaciers")]
    public class SerpentOfTheGlaciers : BaseCreature
    {
        private DateTime m_NextFrostNova;
        private DateTime m_NextCryostasis;
        private DateTime m_NextIceShardVolley;
        private bool m_AbilitiesInitialized;

        [Constructable]
        public SerpentOfTheGlaciers()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "Serpent of the Glaciers";
            Body = 89; // Same body as Ice Serpent
            BaseSoundID = 219;
            Hue = 1150; // Ethereal glacial blue

            SetStr(550, 600);
            SetDex(90, 120);
            SetInt(300, 350);

            SetHits(1600, 1800);
            SetMana(1200);

            SetDamage(20, 30);
            SetDamageType(ResistanceType.Physical, 20);
            SetDamageType(ResistanceType.Cold, 80);

            SetResistance(ResistanceType.Physical, 50, 65);
            SetResistance(ResistanceType.Fire, 10, 20);
            SetResistance(ResistanceType.Cold, 90, 100);
            SetResistance(ResistanceType.Poison, 40, 50);
            SetResistance(ResistanceType.Energy, 50, 65);

            SetSkill(SkillName.Magery, 100.0, 120.0);
            SetSkill(SkillName.EvalInt, 100.0, 120.0);
            SetSkill(SkillName.MagicResist, 100.0, 120.0);
            SetSkill(SkillName.Tactics, 80.0, 100.0);
            SetSkill(SkillName.Wrestling, 80.0, 100.0);

            Fame = 26000;
            Karma = -26000;
            VirtualArmor = 60;

            m_AbilitiesInitialized = false;
        }

        public SerpentOfTheGlaciers(Serial serial)
            : base(serial)
        {
        }

        public override bool AutoDispel => true;
        public override int TreasureMapLevel => 5;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.FilthyRich, 3);
            AddLoot(LootPack.Gems, 5);
            PackItem(new GlacialStaff());

            if (Utility.RandomDouble() < 0.02) // 2% drop
                PackItem(new FrostbindersCloak());
        }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (!m_AbilitiesInitialized)
                {
                    m_NextFrostNova = DateTime.UtcNow + TimeSpan.FromSeconds(10);
                    m_NextCryostasis = DateTime.UtcNow + TimeSpan.FromSeconds(20);
                    m_NextIceShardVolley = DateTime.UtcNow + TimeSpan.FromSeconds(15);
                    m_AbilitiesInitialized = true;
                }

                if (DateTime.UtcNow >= m_NextFrostNova)
                    FrostNova();

                if (DateTime.UtcNow >= m_NextCryostasis)
                    Cryostasis();

                if (DateTime.UtcNow >= m_NextIceShardVolley)
                    IceShardVolley();
            }
        }

        private void FrostNova()
        {
            PublicOverheadMessage(MessageType.Regular, 0x480, true, "* A freezing shockwave erupts from the serpent! *");
            PlaySound(0x64C); // Cold burst sound
            Effects.SendLocationEffect(Location, Map, 0x1F17, 20);

            foreach (Mobile m in GetMobilesInRange(3))
            {
                if (m != this && m.Alive && !m.IsDeadBondedPet)
                {
                    m.Freeze(TimeSpan.FromSeconds(2));
                    AOS.Damage(m, this, Utility.RandomMinMax(30, 45), 0, 100, 0, 0, 0);
                    m.SendMessage("You are caught in the serpent's icy blast!");
                }
            }

            m_NextFrostNova = DateTime.UtcNow + TimeSpan.FromSeconds(25);
        }

        private void Cryostasis()
        {
            if (Combatant is Mobile target && Utility.RandomDouble() < 0.33)
            {
                PublicOverheadMessage(MessageType.Regular, 0x480, true, "* The serpent invokes ancient cryostasis magic! *");
                target.Freeze(TimeSpan.FromSeconds(3));
                Effects.SendTargetEffect(target, 0x376A, 10, 1);
                target.SendMessage("You are locked in time by glacial magic...");

                if (target.Mana > 0)
                    target.Mana -= 10;
                if (target.Stam > 0)
                    target.Stam -= 10;

                m_NextCryostasis = DateTime.UtcNow + TimeSpan.FromSeconds(35);
            }
        }

        private void IceShardVolley()
        {
            PublicOverheadMessage(MessageType.Regular, 0x480, true, "* The serpent launches a volley of glacial shards! *");

            for (int i = 0; i < 6; i++)
            {
                Timer.DelayCall(TimeSpan.FromMilliseconds(300 * i), () =>
                {
                    if (Combatant is Mobile target && target.Alive)
                    {
                        Effects.SendMovingEffect(this, target, 0x1363, 7, 1, false, false, 1153, 0);
                        AOS.Damage(target, this, Utility.RandomMinMax(10, 18), 0, 100, 0, 0, 0);
                        target.SendMessage("A shard of ice pierces you!");
                    }
                });
            }

            m_NextIceShardVolley = DateTime.UtcNow + TimeSpan.FromSeconds(20);
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
            m_AbilitiesInitialized = false;
        }
    }
}
