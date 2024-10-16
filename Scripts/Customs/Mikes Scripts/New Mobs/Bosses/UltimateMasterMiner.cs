using System;
using System.Collections;
using Server.Engines.CannedEvil;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("corpse of King Solomon")]
    public class UltimateMasterMiner : BaseChampion
    {
        private DateTime m_NextAbilityTime;

        [Constructable]
        public UltimateMasterMiner()
            : base(AIType.AI_Melee)
        {
            Name = "King Solomon";
            Title = "The Wisest King";
            Body = 0x190;
            Hue = 0x83F;

            SetStr(305, 425);
            SetDex(72, 150);
            SetInt(505, 750);

            SetHits(15000);
            SetMana(2500);

            SetDamage(30, 40);

            SetDamageType(ResistanceType.Physical, 50);

            SetResistance(ResistanceType.Physical, 60, 70);
            SetResistance(ResistanceType.Fire, 60, 70);
            SetResistance(ResistanceType.Cold, 60, 70);
            SetResistance(ResistanceType.Poison, 60, 70);
            SetResistance(ResistanceType.Energy, 50, 60);

            SetSkill(SkillName.Mining, 120.0);
            SetSkill(SkillName.Tactics, 120.0);
            SetSkill(SkillName.Wrestling, 120.0);
            SetSkill(SkillName.Anatomy, 120.0);
            SetSkill(SkillName.MagicResist, 120.0);

            Fame = 22500;
            Karma = -22500;

            VirtualArmor = 70;

            AddItem(new FancyShirt(Utility.RandomGreenHue()));
            AddItem(new LongPants(Utility.RandomYellowHue()));
            AddItem(new Cloak(Utility.RandomPinkHue()));
            AddItem(new Sandals());

            HairItemID = 0x203B; // Short Hair
            HairHue = 0x94;
        }

        public UltimateMasterMiner(Serial serial)
            : base(serial)
        {
        }

        public override ChampionSkullType SkullType { get { return ChampionSkullType.Power; } }

        public override Type[] UniqueList
        {
            get { return new Type[] { typeof(PickaxeOfWealth), typeof(KingsCrown) }; }
        }

        public override Type[] SharedList
        {
            get { return new Type[] { typeof(GoldenOre), typeof(SolomonsTome) }; }
        }

        public override Type[] DecorativeList
        {
            get { return new Type[] { typeof(GoldenStatue), typeof(MiningTools) }; }
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

            c.DropItem(new PowerScroll(SkillName.Mining, 200.0));

            if (Utility.RandomDouble() < 0.6)
                c.DropItem(new PickaxeOfWealth());

            if (Utility.RandomDouble() < 0.6)
                c.DropItem(new KingsCrown());
        }

        public override void OnGaveMeleeAttack(Mobile defender)
        {
            base.OnGaveMeleeAttack(defender);

            if (DateTime.Now > m_NextAbilityTime)
            {
                switch (Utility.Random(3))
                {
                    case 0: Earthquake(); break;
                    case 1: GoldenTouch(defender); break;
                    case 2: OreStrike(defender); break;
                }

                m_NextAbilityTime = DateTime.Now + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            }
        }

        public void Earthquake()
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

                int damage = Utility.RandomMinMax(80, 100);

                AOS.Damage(m, this, damage, 100, 0, 0, 0, 0);

                m.FixedParticles(0x36BD, 20, 10, 5044, EffectLayer.Head);
                m.PlaySound(0x207);
                m.Paralyze(TimeSpan.FromSeconds(3.0));
            }
        }

        public void GoldenTouch(Mobile defender)
        {
            if (defender != null)
            {
                defender.FixedParticles(0x376A, 9, 32, 5007, EffectLayer.Waist);
                defender.PlaySound(0x1FA);

                defender.Paralyze(TimeSpan.FromSeconds(5.0));
                defender.SendMessage("You have been turned to gold and immobilized!");
            }
        }

        public void OreStrike(Mobile defender)
        {
            if (defender != null)
            {
                int damage = Utility.RandomMinMax(100, 150);

                AOS.Damage(defender, this, damage, 100, 0, 0, 0, 0);

                defender.FixedParticles(0x36BD, 20, 10, 5044, EffectLayer.Head);
                defender.PlaySound(0x207);
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
