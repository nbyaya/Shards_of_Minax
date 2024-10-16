using System;
using System.Collections;
using Server.Engines.CannedEvil;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("corpse of Charles Darwin")]
    public class UltimateMasterAnimalLore : BaseChampion
    {
        private DateTime m_NextAbilityTime;

        [Constructable]
        public UltimateMasterAnimalLore()
            : base(AIType.AI_Mage)
        {
            Name = "Charles Darwin";
            Title = "The Father of Evolution";
            Body = 0x190;
            Hue = 0x83F;

            SetStr(305, 425);
            SetDex(72, 150);
            SetInt(505, 750);

            SetHits(4800);
            SetMana(2500);

            SetDamage(25, 35);

            SetDamageType(ResistanceType.Physical, 25);
            SetDamageType(ResistanceType.Fire, 25);
            SetDamageType(ResistanceType.Cold, 25);
            SetDamageType(ResistanceType.Poison, 25);

            SetResistance(ResistanceType.Physical, 60, 70);
            SetResistance(ResistanceType.Fire, 60, 70);
            SetResistance(ResistanceType.Cold, 60, 70);
            SetResistance(ResistanceType.Poison, 60, 70);
            SetResistance(ResistanceType.Energy, 60, 70);

            SetSkill(SkillName.AnimalLore, 120.0);
            SetSkill(SkillName.AnimalTaming, 120.0);
            SetSkill(SkillName.Veterinary, 120.0);
            SetSkill(SkillName.MagicResist, 120.0);
            SetSkill(SkillName.Tactics, 120.0);
            SetSkill(SkillName.Wrestling, 120.0);

            Fame = 22500;
            Karma = -22500;

            VirtualArmor = 70;
			
            AddItem(new FancyShirt(Utility.RandomGreenHue()));
            AddItem(new LongPants(Utility.RandomYellowHue()));
            AddItem(new Cloak(Utility.RandomPinkHue()));
            AddItem(new Sandals());

            HairItemID = 0x203B; // Short Hair
            HairHue = 0x44E;
        }

        public UltimateMasterAnimalLore(Serial serial)
            : base(serial)
        {
        }

        public override ChampionSkullType SkullType { get { return ChampionSkullType.Power; } }

        public override Type[] UniqueList
        {
            get { return new Type[] { typeof(EvolutionaryTome), typeof(DarwinsSpecimenJar) }; }
        }

        public override Type[] SharedList
        {
            get { return new Type[] { typeof(EvolutionaryTome), typeof(DarwinsSpecimenJar) }; }
        }

        public override Type[] DecorativeList
        {
            get { return new Type[] { typeof(EvolutionaryTome), typeof(DarwinsSpecimenJar) }; }
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

            c.DropItem(new PowerScroll(SkillName.AnimalLore, 200.0));

            if (Utility.RandomDouble() < 0.6)
                c.DropItem(new EvolutionaryTome());

            if (Utility.RandomDouble() < 0.6)
                c.DropItem(new DarwinsSpecimenJar());
        }

        public override void OnGaveMeleeAttack(Mobile defender)
        {
            base.OnGaveMeleeAttack(defender);

            if (DateTime.Now > m_NextAbilityTime)
            {
                switch (Utility.Random(3))
                {
                    case 0: BeastCall(); break;
                    case 1: Adaptation(); break;
                    case 2: NaturalSelection(defender); break;
                }

                m_NextAbilityTime = DateTime.Now + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            }
        }

        public void BeastCall()
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

                BaseCreature creature = null;

                switch (Utility.Random(3))
                {
                    case 0: creature = new DireWolf(); break;
                    case 1: creature = new HellHound(); break;
                    case 2: creature = new WhiteWyrm(); break;
                }

                if (creature != null)
                {
                    creature.Team = this.Team;
                    creature.Combatant = m;
                    creature.MoveToWorld(this.Location, this.Map);
                }
            }
        }

        public void Adaptation()
        {
            this.Say("Adapting to a new resistance!");

            switch (Utility.Random(5))
            {
                case 0: this.SetResistance(ResistanceType.Physical, 90); break;
                case 1: this.SetResistance(ResistanceType.Fire, 90); break;
                case 2: this.SetResistance(ResistanceType.Cold, 90); break;
                case 3: this.SetResistance(ResistanceType.Poison, 90); break;
                case 4: this.SetResistance(ResistanceType.Energy, 90); break;
            }
        }

        public void NaturalSelection(Mobile defender)
        {
            if (defender != null)
            {
                defender.FixedParticles(0x376A, 9, 32, 5007, EffectLayer.Waist);
                defender.PlaySound(0x1FA);

                defender.SendMessage("You have been selected by nature!");

                defender.Str -= 10;
                defender.Dex -= 10;
                defender.Int -= 10;
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
}
