using System;
using System.Collections;
using Server.Engines.CannedEvil;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("corpse of Charles Dickens")]
    public class UltimateMasterBeggar : BaseChampion
    {
        private DateTime m_NextAbilityTime;

        [Constructable]
        public UltimateMasterBeggar()
            : base(AIType.AI_Melee)
        {
            Name = "Charles Dickens";
            Title = "as Oliver Twist";
            Body = 0x190;
            Hue = 0x83F2;

            SetStr(105, 225);
            SetDex(200, 250);
            SetInt(205, 300);

            SetHits(10000);
            SetMana(1000);

            SetDamage(15, 25);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 60, 70);
            SetResistance(ResistanceType.Fire, 50, 60);
            SetResistance(ResistanceType.Cold, 50, 60);
            SetResistance(ResistanceType.Poison, 50, 60);
            SetResistance(ResistanceType.Energy, 50, 60);

            SetSkill(SkillName.Begging, 120.0);
            SetSkill(SkillName.Tactics, 100.0);
            SetSkill(SkillName.Wrestling, 100.0);
            SetSkill(SkillName.Stealing, 120.0);
            SetSkill(SkillName.Snooping, 100.0);

            Fame = 22500;
            Karma = 22500;

            VirtualArmor = 60;
			
            AddItem(new FancyShirt(1157));
            AddItem(new ShortPants(1741));
            AddItem(new Boots(1908));
            AddItem(new SkullCap(1741));

            HairItemID = 0x203B; // Short Hair
            HairHue = 1150;
        }

        public UltimateMasterBeggar(Serial serial)
            : base(serial)
        {
        }

        public override ChampionSkullType SkullType { get { return ChampionSkullType.Power; } }

        public override Type[] UniqueList
        {
            get { return new Type[] { typeof(OliversBowl), typeof(RagsToRichesCloak) }; }
        }

        public override Type[] SharedList
        {
            get { return new Type[] { typeof(BeggarsCoin), typeof(PocketWatch) }; }
        }

        public override Type[] DecorativeList
        {
            get { return new Type[] { typeof(OliversBowl), typeof(TatteredHat) }; }
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

            c.DropItem(new PowerScroll(SkillName.Begging, 200.0));

            if (Utility.RandomDouble() < 0.6)
                c.DropItem(new OliversBowl());

            if (Utility.RandomDouble() < 0.6)
                c.DropItem(new RagsToRichesCloak());
        }

        public override void OnGaveMeleeAttack(Mobile defender)
        {
            base.OnGaveMeleeAttack(defender);

            if (DateTime.Now > m_NextAbilityTime)
            {
                switch (Utility.Random(3))
                {
                    case 0: PitifulPlea(defender); break;
                    case 1: DistractingWhine(defender); break;
                    case 2: Hoarder(defender); break;
                }

                m_NextAbilityTime = DateTime.Now + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            }
        }

        public void PitifulPlea(Mobile defender)
        {
            if (defender != null)
            {
                defender.SendLocalizedMessage(1070846); // The creature's pitiful plea tugs at your heartstrings, lowering your defenses.
                defender.AddStatMod(new StatMod(StatType.All, "PitifulPlea", -20, TimeSpan.FromSeconds(10)));
                defender.FixedParticles(0x375A, 9, 20, 5027, EffectLayer.Waist);
                defender.PlaySound(0x1F9);
            }
        }

        public void DistractingWhine(Mobile defender)
        {
            if (defender != null)
            {
                defender.SendLocalizedMessage(1070847); // The creature's incessant whining stuns you momentarily.
                defender.Freeze(TimeSpan.FromSeconds(3));
                defender.FixedParticles(0x376A, 9, 32, 5030, EffectLayer.Waist);
                defender.PlaySound(0x204);
            }
        }

        public void Hoarder(Mobile defender)
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

                m.SendLocalizedMessage(1070848); // The creature pilfers gold from your backpack!
                m.PlaySound(0x2E6);

                int toSteal = Utility.RandomMinMax(100, 300);
                m.Backpack.ConsumeTotal(typeof(Gold), toSteal);
                this.AddToBackpack(new Gold(toSteal));
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

    public class OliversBowl : Item
    {
        [Constructable]
        public OliversBowl() : base(0x15FD)
        {
            Name = "Oliver's Bowl";
            Hue = 0x47E;
            Weight = 1.0;
        }

        public OliversBowl(Serial serial) : base(serial)
        {
        }

        public override void OnDoubleClick(Mobile from)
        {
            from.SendMessage("This bowl increases your chance of success in begging.");
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

    public class RagsToRichesCloak : BaseCloak
    {
        [Constructable]
        public RagsToRichesCloak() : base(0x1515)
        {
            Name = "Rags to Riches Cloak";
            Hue = 0x481;
            Weight = 5.0;
        }

        public RagsToRichesCloak(Serial serial) : base(serial)
        {
        }

        public override void OnDoubleClick(Mobile from)
        {
            from.SendMessage("This cloak boosts your luck.");
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
	
	
	public class BeggarsCoin : Item
	{
		[Constructable]
		public BeggarsCoin() : base(0xEED) // Coin graphic
		{
			Name = "Beggar's Coin";
			Hue = 0x835; // Unique color for the coin
			Weight = 0.1;
		}

		public BeggarsCoin(Serial serial) : base(serial)
		{
		}

		public override void OnDoubleClick(Mobile from)
		{
			from.SendMessage("This coin brings fortune to those in need.");
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
	
	public class PocketWatch : Item
	{
		[Constructable]
		public PocketWatch() : base(0x1CDB) // Pocket Watch graphic
		{
			Name = "Old Pocket Watch";
			Hue = 0x835; // Unique color for the watch
			Weight = 1.0;
		}

		public PocketWatch(Serial serial) : base(serial)
		{
		}

		public override void OnDoubleClick(Mobile from)
		{
			from.SendMessage("Time is of the essence, and this watch gives you an edge.");
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
	
	public class TatteredHat : BaseHat
	{
		[Constructable]
		public TatteredHat() : base(0x1718) // Hat graphic
		{
			Name = "Tattered Hat";
			Hue = 0x47E; // Unique color for the hat
			Weight = 1.0;
		}

		public TatteredHat(Serial serial) : base(serial)
		{
		}

		public override void OnDoubleClick(Mobile from)
		{
			from.SendMessage("Wearing this hat reminds you of humble beginnings.");
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