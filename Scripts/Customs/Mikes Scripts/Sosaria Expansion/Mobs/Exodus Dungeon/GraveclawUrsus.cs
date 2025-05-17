using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Spells;
using Server.Targeting;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("the remains of Graveclaw Ursus")]
    public class GraveclawUrsus : BaseCreature
    {
        private DateTime m_NextRoar;
        private DateTime m_NextSoulDrain;
        private DateTime m_NextEntomb;
        private bool m_AbilitiesInitialized;

        [Constructable]
        public GraveclawUrsus()
            : base(AIType.AI_Melee, FightMode.Closest, 12, 1, 0.2, 0.4)
        {
            Name = "Graveclaw Ursus";
            Body = 211; // Bear body
            Hue = 2955; // Ghostly frost-glow hue
            BaseSoundID = 0xA3; // Bear sound

            SetStr(1200, 1400);
            SetDex(90, 120);
            SetInt(350, 400);

            SetHits(1800, 2200);
            SetDamage(30, 40);

            SetDamageType(ResistanceType.Physical, 40);
            SetDamageType(ResistanceType.Cold, 30);
            SetDamageType(ResistanceType.Poison, 30);

            SetResistance(ResistanceType.Physical, 65, 75);
            SetResistance(ResistanceType.Cold, 80, 90);
            SetResistance(ResistanceType.Fire, 25, 35);
            SetResistance(ResistanceType.Poison, 70, 85);
            SetResistance(ResistanceType.Energy, 35, 45);

            SetSkill(SkillName.MagicResist, 120.0, 150.0);
            SetSkill(SkillName.Tactics, 100.0, 120.0);
            SetSkill(SkillName.Wrestling, 100.0, 120.0);
            SetSkill(SkillName.SpiritSpeak, 80.0, 100.0);

            Fame = 24000;
            Karma = -24000;

            VirtualArmor = 75;

            m_AbilitiesInitialized = false;
        }

        public GraveclawUrsus(Serial serial)
            : base(serial)
        {
        }

        public override bool ReacquireOnMovement => !Controlled;
        public override bool AutoDispel => !Controlled;
        public override bool BleedImmune => true;
        public override int Meat => 2;
        public override int Hides => 18;
        public override FoodType FavoriteFood => FoodType.Meat;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 2);
            AddLoot(LootPack.Gems, 6);

            if (Utility.RandomDouble() < 0.05)
                PackItem(new GraveclawPendant());
        }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant == null)
                return;

            if (!m_AbilitiesInitialized)
            {
                m_NextRoar = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 20));
                m_NextSoulDrain = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 35));
                m_NextEntomb = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(30, 45));
                m_AbilitiesInitialized = true;
            }

            if (DateTime.UtcNow >= m_NextRoar)
                DeathRoar();

            if (DateTime.UtcNow >= m_NextSoulDrain)
                SoulDrain();

            if (DateTime.UtcNow >= m_NextEntomb)
                Entomb();
        }

        private void DeathRoar()
        {
            PublicOverheadMessage(MessageType.Regular, 0x22, true, "*Graveclaw Ursus releases a deathly roar!*");
            PlaySound(0x50F); // Death roar sound
            Effects.SendLocationEffect(Location, Map, 0x3709, 20, 10);

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive && CanBeHarmful(m))
                {
                    m.FixedParticles(0x3728, 1, 30, 9964, 92, 3, EffectLayer.Waist);
                    m.SendMessage("The roar rattles your soul!");
                    m.Stam -= Utility.Random(15, 25);
                    m.Paralyze(TimeSpan.FromSeconds(2));
                }
            }

            m_NextRoar = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(40, 60));
        }

        private void SoulDrain()
        {
            Mobile target = Combatant as Mobile;
            if (target != null && CanBeHarmful(target))
            {
                PublicOverheadMessage(MessageType.Regular, 0x22, true, "*Graveclaw drains the warmth of your spirit!*");
                target.FixedParticles(0x376A, 10, 25, 5032, EffectLayer.Head);
                PlaySound(0x1F2);

                int damage = Utility.RandomMinMax(30, 60);
                AOS.Damage(target, this, damage, 0, 0, 0, 100, 0);

                Hits += damage / 2;

                m_NextSoulDrain = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(35, 60));
            }
        }

        private void Entomb()
        {
            PublicOverheadMessage(MessageType.Regular, 0x22, true, "*The earth cracks and grave frost erupts!*");
            Effects.PlaySound(Location, Map, 0x64F);

            foreach (Mobile m in GetMobilesInRange(4))
            {
                if (m != this && m.Alive && CanBeHarmful(m))
                {
                    m.Freeze(TimeSpan.FromSeconds(3));
                    AOS.Damage(m, this, Utility.RandomMinMax(15, 30), 0, 0, 0, 100, 0);
                    Effects.SendTargetEffect(m, 0x376A, 1, 30, 0, 3);
                    m.SendMessage("Frozen tombs rise around you, binding your feet!");
                }
            }

            m_NextEntomb = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(45, 75));
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_AbilitiesInitialized = false;
        }
    }

    public class GraveclawPendant : Item
    {
        [Constructable]
        public GraveclawPendant() : base(0x1F09)
        {
            Name = "Pendant of Graveclaw";
            Hue = 2955;
            LootType = LootType.Blessed;
        }

        public GraveclawPendant(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
