using System;
using System.Collections;
using Server.Engines.CannedEvil;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("corpse of Ptolemy")]
    public class UltimateMasterCartographer : BaseChampion
    {
        private DateTime m_NextAbilityTime;

        [Constructable]
        public UltimateMasterCartographer()
            : base(AIType.AI_Mage)
        {
            Name = "Ptolemy";
            Title = "The Great Cartographer";
            Body = 0x190;
            Hue = 0x83F;

            SetStr(305, 425);
            SetDex(72, 150);
            SetInt(505, 750);

            SetHits(12000);
            SetMana(2500);

            SetDamage(25, 35);

            SetDamageType(ResistanceType.Physical, 25);
            SetDamageType(ResistanceType.Fire, 25);
            SetDamageType(ResistanceType.Cold, 25);
            SetDamageType(ResistanceType.Poison, 25);

            SetResistance(ResistanceType.Physical, 60, 70);
            SetResistance(ResistanceType.Fire, 60, 70);
            SetResistance(ResistanceType.Cold, 60, 70);
            SetResistance(ResistanceType.Poison, 50, 60);
            SetResistance(ResistanceType.Energy, 50, 60);

            SetSkill(SkillName.EvalInt, 120.0);
            SetSkill(SkillName.Magery, 120.0);
            SetSkill(SkillName.Meditation, 120.0);
            SetSkill(SkillName.Cartography, 120.0);
            SetSkill(SkillName.MagicResist, 120.0);
            SetSkill(SkillName.Tactics, 120.0);
            SetSkill(SkillName.Wrestling, 120.0);

            Fame = 22500;
            Karma = -22500;

            VirtualArmor = 70;

            AddItem(new Robe(Utility.RandomBlueHue()));
            AddItem(new Sandals());
            AddItem(new Cloak(Utility.RandomYellowHue()));
            AddItem(new WizardsHat());

            HairItemID = 0x203C; // Long Hair
            HairHue = 0x47E;
        }

        public UltimateMasterCartographer(Serial serial)
            : base(serial)
        {
        }

        public override ChampionSkullType SkullType { get { return ChampionSkullType.Power; } }

        public override Type[] UniqueList
        {
            get { return new Type[] { typeof(AncientMap), typeof(CompassOfTheAges) }; }
        }

        public override Type[] SharedList
        {
            get { return new Type[] { typeof(MapOfTheWorld), typeof(CartographerTools) }; }
        }

        public override Type[] DecorativeList
        {
            get { return new Type[] { typeof(MapOfTheWorld), typeof(CartographerGlobe) }; }
        }

        public override MonsterStatuetteType[] StatueTypes
        {
            get { return new MonsterStatuetteType[] { }; }
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 3);
            AddLoot(LootPack.FilthyRich);
            AddLoot(LootPack.Gems, 6);
        }

        public override void OnDeath(Container c)
        {
            base.OnDeath(c);

            c.DropItem(new PowerScroll(SkillName.Cartography, 200.0));

            if (Utility.RandomDouble() < 0.6)
                c.DropItem(new AncientMap());

            if (Utility.RandomDouble() < 0.6)
                c.DropItem(new CompassOfTheAges());
        }

        public override void OnGaveMeleeAttack(Mobile defender)
        {
            base.OnGaveMeleeAttack(defender);

            if (DateTime.Now > m_NextAbilityTime)
            {
                switch (Utility.Random(3))
                {
                    case 0: MapOfTheStars(); break;
                    case 1: FogOfWar(); break;
                    case 2: TerrainShift(); break;
                }

                m_NextAbilityTime = DateTime.Now + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            }
        }

        public void MapOfTheStars()
        {
            ArrayList targets = new ArrayList();

            foreach (Mobile m in this.GetMobilesInRange(8))
            {
                if (m != this && m.Player && this.CanBeBeneficial(m))
                    targets.Add(m);
            }

            for (int i = 0; i < targets.Count; ++i)
            {
                Mobile m = (Mobile)targets[i];

                DoBeneficial(m);

                // Apply a buff effect here, you may need to create a custom buff
                m.FixedParticles(0x375A, 10, 30, 5037, EffectLayer.Waist);
                m.PlaySound(0x1F2);
            }
        }

        public void FogOfWar()
        {
            ArrayList targets = new ArrayList();

            foreach (Mobile m in this.GetMobilesInRange(8))
            {
                if (m != this && m.Player && this.CanBeHarmful(m))
                    targets.Add(m);
            }

            for (int i = 0; i < targets.Count; ++i)
            {
                Mobile m = (Mobile)targets[i];

                DoHarmful(m);

                // Apply a vision reduction effect here, possibly using a custom debuff
                m.FixedParticles(0x375A, 10, 30, 5037, EffectLayer.Waist);
                m.PlaySound(0x1F2);
            }
        }

        public void TerrainShift()
        {
            // Alter the battlefield, creating obstacles.
            // You will need to define how obstacles are created and handled in your game environment
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

    public class AncientMap : Item
    {
        [Constructable]
        public AncientMap() : base(0x14EC)
        {
            Name = "Ancient Map";
            Hue = 0x482;
        }

        public AncientMap(Serial serial) : base(serial)
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

    public class CompassOfTheAges : Item
    {
        [Constructable]
        public CompassOfTheAges() : base(0x14F5)
        {
            Name = "Compass of the Ages";
            Hue = 0x482;
        }

        public CompassOfTheAges(Serial serial) : base(serial)
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
