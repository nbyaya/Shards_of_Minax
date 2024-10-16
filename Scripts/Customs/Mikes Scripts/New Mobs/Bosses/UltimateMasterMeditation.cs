using System;
using System.Collections;
using Server.Engines.CannedEvil;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("corpse of Buddha")]
    public class UltimateMasterMeditation : BaseChampion
    {
        private DateTime m_NextAbilityTime;

        [Constructable]
        public UltimateMasterMeditation()
            : base(AIType.AI_Mage)
        {
            Name = "Buddha";
            Title = "The Enlightened One";
            Body = 0x190;
            Hue = 0x83F;

            SetStr(250, 350);
            SetDex(150, 200);
            SetInt(700, 850);

            SetHits(10000);
            SetMana(5000);

            SetDamage(15, 25);

            SetDamageType(ResistanceType.Physical, 10);
            SetDamageType(ResistanceType.Energy, 90);

            SetResistance(ResistanceType.Physical, 50, 60);
            SetResistance(ResistanceType.Fire, 40, 50);
            SetResistance(ResistanceType.Cold, 40, 50);
            SetResistance(ResistanceType.Poison, 60, 70);
            SetResistance(ResistanceType.Energy, 80, 90);

            SetSkill(SkillName.EvalInt, 120.0);
            SetSkill(SkillName.Magery, 120.0);
            SetSkill(SkillName.Meditation, 120.0);
            SetSkill(SkillName.MagicResist, 120.0);
            SetSkill(SkillName.Tactics, 100.0);
            SetSkill(SkillName.Wrestling, 100.0);

            Fame = 25000;
            Karma = 25000;

            VirtualArmor = 60;
            
            AddItem(new Robe(Utility.RandomBlueHue()));
            AddItem(new Sandals());

            HairItemID = 0x203B; // Short Hair
            HairHue = 0x47E;
        }

        public UltimateMasterMeditation(Serial serial)
            : base(serial)
        {
        }

        public override ChampionSkullType SkullType { get { return ChampionSkullType.Power; } }

        public override Type[] UniqueList
        {
            get { return new Type[] { typeof(LotusBlossom), typeof(MonksRobe) }; }
        }

        public override Type[] SharedList
        {
            get { return new Type[] { typeof(MeditationTome), typeof(MysticalIncense) }; }
        }

        public override Type[] DecorativeList
        {
            get { return new Type[] { typeof(LotusBlossom), typeof(MeditationStatue) }; }
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

            c.DropItem(new PowerScroll(SkillName.Meditation, 200.0));

            if (Utility.RandomDouble() < 0.6)
                c.DropItem(new MeditationTome());

            if (Utility.RandomDouble() < 0.6)
                c.DropItem(new MysticalIncense());
        }

        public override void OnGaveMeleeAttack(Mobile defender)
        {
            base.OnGaveMeleeAttack(defender);

            if (DateTime.Now > m_NextAbilityTime)
            {
                switch (Utility.Random(3))
                {
                    case 0: InnerPeace(); break;
                    case 1: ManaSurge(); break;
                    case 2: Tranquility(); break;
                }

                m_NextAbilityTime = DateTime.Now + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            }
        }

        public void InnerPeace()
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

                m.Hits += Utility.RandomMinMax(50, 70);
                m.Paralyzed = false;
                m.Poison = null;

                m.FixedParticles(0x373A, 10, 15, 5018, EffectLayer.Head);
                m.PlaySound(0x1F2);
            }
        }

        public void ManaSurge()
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

                m.Mana += Utility.RandomMinMax(100, 150);

                m.FixedParticles(0x375A, 9, 20, 5021, EffectLayer.Head);
                m.PlaySound(0x1F4);
            }
        }

        public void Tranquility()
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

                m.VirtualArmorMod += 20;
                Timer.DelayCall(TimeSpan.FromSeconds(20.0), delegate { m.VirtualArmorMod -= 20; });

                m.FixedParticles(0x376A, 9, 32, 5030, EffectLayer.Waist);
                m.PlaySound(0x1F3);
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


	public class LotusBlossom : Item
	{
		[Constructable]
		public LotusBlossom() : base(0x1F33)
		{
			Hue = 0x48E;
			Name = "Lotus Blossom";
		}

		public LotusBlossom(Serial serial) : base(serial)
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

	public class MonksRobe : BaseClothing
	{
		[Constructable]
		public MonksRobe() : base(0x1F04, Layer.OuterTorso, 0x1)
		{
			Hue = 0x1;
			Name = "Monk's Robe";
			Attributes.RegenMana = 2;
			Attributes.LowerManaCost = 5;
		}

		public MonksRobe(Serial serial) : base(serial)
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

	public class MeditationTome : Item
	{
		[Constructable]
		public MeditationTome() : base(0x1F4D)
		{
			Hue = 0x1;
			Name = "Tome of Meditation";
		}

		public MeditationTome(Serial serial) : base(serial)
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

	public class MysticalIncense : Item
	{
		[Constructable]
		public MysticalIncense() : base(0x142E)
		{
			Hue = 0x1;
			Name = "Mystical Incense";
		}

		public MysticalIncense(Serial serial) : base(serial)
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

	public class MeditationStatue : Item
	{
		[Constructable]
		public MeditationStatue() : base(0x122C)
		{
			Hue = 0x1;
			Name = "Statue of Meditation";
		}

		public MeditationStatue(Serial serial) : base(serial)
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
