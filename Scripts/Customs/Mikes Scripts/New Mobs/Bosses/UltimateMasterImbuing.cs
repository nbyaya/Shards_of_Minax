using System;
using System.Collections;
using Server.Engines.CannedEvil;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("corpse of Nicolas Flamel")]
    public class UltimateMasterImbuing : BaseChampion
    {
        private DateTime m_NextAbilityTime;

        [Constructable]
        public UltimateMasterImbuing()
            : base(AIType.AI_Mage)
        {
            Name = "Nicolas Flamel";
            Title = "The Legendary Alchemist";
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
            SetResistance(ResistanceType.Poison, 90, 100);
            SetResistance(ResistanceType.Energy, 50, 60);

            SetSkill(SkillName.EvalInt, 120.0);
            SetSkill(SkillName.Magery, 120.0);
            SetSkill(SkillName.Meditation, 120.0);
            SetSkill(SkillName.Imbuing, 120.0);
            SetSkill(SkillName.MagicResist, 120.0);
            SetSkill(SkillName.Tactics, 120.0);
            SetSkill(SkillName.Wrestling, 120.0);

            Fame = 22500;
            Karma = -22500;

            VirtualArmor = 70;

            AddItem(new Robe(Utility.RandomRedHue()));
            AddItem(new Sandals());
            AddItem(new WizardsHat());

            HairItemID = 0x203C; // Long Hair
            HairHue = 0x47E;
        }

        public UltimateMasterImbuing(Serial serial)
            : base(serial)
        {
        }

        public override ChampionSkullType SkullType { get { return ChampionSkullType.Power; } }

        public override Type[] UniqueList
        {
            get { return new Type[] { typeof(AlchemistsRing), typeof(PhilosophersStone) }; }
        }

        public override Type[] SharedList
        {
            get { return new Type[] { typeof(AncientImbuerTome), typeof(MagicEssence) }; }
        }

        public override Type[] DecorativeList
        {
            get { return new Type[] { typeof(MysticalGem), typeof(ArcaneOrb) }; }
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

            c.DropItem(new PowerScroll(SkillName.Imbuing, 200.0));

            if (Utility.RandomDouble() < 0.6)
                c.DropItem(new AlchemistsRing());

            if (Utility.RandomDouble() < 0.6)
                c.DropItem(new PhilosophersStone());
        }

        public override void OnGaveMeleeAttack(Mobile defender)
        {
            base.OnGaveMeleeAttack(defender);

            if (DateTime.Now > m_NextAbilityTime)
            {
                switch (Utility.Random(3))
                {
                    case 0: Enchant(); break;
                    case 1: Transmutation(defender); break;
                    case 2: ArcaneExplosion(); break;
                }

                m_NextAbilityTime = DateTime.Now + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            }
        }

        public void Enchant()
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

                m.FixedParticles(0x375A, 10, 15, 5018, EffectLayer.Waist);
                m.PlaySound(0x1F2);
                m.SendMessage("Your weapon glows with a mystical energy!");

                // Logic to temporarily boost weapon damage or attributes
            }
        }

        public void Transmutation(Mobile defender)
        {
            if (defender != null)
            {
                defender.FixedParticles(0x374A, 10, 15, 5021, EffectLayer.Waist);
                defender.PlaySound(0x1FB);
                defender.SendMessage("You feel your resistances shifting!");

                // Logic to change enemy resistances
            }
        }

        public void ArcaneExplosion()
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

                int damage = Utility.RandomMinMax(60, 80);

                AOS.Damage(m, this, damage, 0, 0, 100, 0, 0);

                m.FixedParticles(0x36BD, 20, 10, 5044, EffectLayer.Head);
                m.PlaySound(0x307);
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


	public class AlchemistsRing : Item
	{
		[Constructable]
		public AlchemistsRing() : base(0x1088)
		{
			Name = "Alchemist's Ring";
			Hue = 0x501;
		}

		public AlchemistsRing(Serial serial) : base(serial)
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

	public class PhilosophersStone : Item
	{
		[Constructable]
		public PhilosophersStone() : base(0x1F19)
		{
			Name = "Philosopher's Stone";
			Hue = 0x489;
		}

		public PhilosophersStone(Serial serial) : base(serial)
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

	public class AncientImbuerTome : Item
	{
		[Constructable]
		public AncientImbuerTome() : base(0x1C12)
		{
			Name = "Ancient Imbuer's Tome";
			Hue = 0x48E;
		}

		public AncientImbuerTome(Serial serial) : base(serial)
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

	public class MagicEssence : Item
	{
		[Constructable]
		public MagicEssence() : base(0x1F1C)
		{
			Name = "Magic Essence";
			Hue = 0x48F;
		}

		public MagicEssence(Serial serial) : base(serial)
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

	public class MysticalGem : Item
	{
		[Constructable]
		public MysticalGem() : base(0x1EA7)
		{
			Name = "Mystical Gem";
			Hue = 0x48D;
		}

		public MysticalGem(Serial serial) : base(serial)
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

	public class ArcaneOrb : Item
	{
		[Constructable]
		public ArcaneOrb() : base(0x1F13)
		{
			Name = "Arcane Orb";
			Hue = 0x490;
		}

		public ArcaneOrb(Serial serial) : base(serial)
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
