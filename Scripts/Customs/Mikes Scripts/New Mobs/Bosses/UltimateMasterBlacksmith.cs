using System;
using System.Collections;
using Server.Engines.CannedEvil;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("corpse of Hephaestus")]
    public class UltimateMasterBlacksmith : BaseChampion
    {
        private DateTime m_NextAbilityTime;

        [Constructable]
        public UltimateMasterBlacksmith()
            : base(AIType.AI_Melee)
        {
            Name = "Hephaestus";
            Title = "God of the Forge";
            Body = 0x190;
            Hue = 0x83F;

            SetStr(405, 525);
            SetDex(102, 150);
            SetInt(305, 450);

            SetHits(15000);
            SetMana(2000);

            SetDamage(30, 40);

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Fire, 50);

            SetResistance(ResistanceType.Physical, 70, 80);
            SetResistance(ResistanceType.Fire, 80, 90);
            SetResistance(ResistanceType.Cold, 60, 70);
            SetResistance(ResistanceType.Poison, 60, 70);
            SetResistance(ResistanceType.Energy, 60, 70);

            SetSkill(SkillName.Blacksmith, 120.0);
            SetSkill(SkillName.Macing, 120.0);
            SetSkill(SkillName.Tactics, 120.0);
            SetSkill(SkillName.Wrestling, 120.0);
            SetSkill(SkillName.Parry, 120.0);

            Fame = 25000;
            Karma = 25000;

            VirtualArmor = 80;
			
            AddItem(new FancyShirt(1157));
            AddItem(new LongPants(1908));
            AddItem(new FullApron(1908));
            AddItem(new Sandals());

            HairItemID = 0x203B; // Short Hair
            HairHue = 0x3B2;

            FacialHairItemID = 0x203E; // Short Beard
            FacialHairHue = 0x3B2;
        }

        public UltimateMasterBlacksmith(Serial serial)
            : base(serial)
        {
        }

        public override ChampionSkullType SkullType { get { return ChampionSkullType.Power; } }

        public override Type[] UniqueList
        {
            get { return new Type[] { typeof(ForgeHammer), typeof(ShieldOfTheForge) }; }
        }

        public override Type[] SharedList
        {
            get { return new Type[] { typeof(HephaestusHammer), typeof(VolcanicOre) }; }
        }

        public override Type[] DecorativeList
        {
            get { return new Type[] { typeof(ForgeHammer), typeof(AncientAnvil) }; }
        }

        public override MonsterStatuetteType[] StatueTypes
        {
            get { return new MonsterStatuetteType[] { }; }
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 3);
            AddLoot(LootPack.FilthyRich);
            AddLoot(LootPack.Gems, 8);
        }

        public override void OnDeath(Container c)
        {
            base.OnDeath(c);

            c.DropItem(new PowerScroll(SkillName.Blacksmith, 200.0));

            if (Utility.RandomDouble() < 0.6)
                c.DropItem(new ForgeHammer());

            if (Utility.RandomDouble() < 0.6)
                c.DropItem(new ShieldOfTheForge());
        }

        public override void OnGaveMeleeAttack(Mobile defender)
        {
            base.OnGaveMeleeAttack(defender);

            if (DateTime.Now > m_NextAbilityTime)
            {
                switch (Utility.Random(3))
                {
                    case 0: MoltenStrike(defender); break;
                    case 1: ForgeArmor(); break;
                    case 2: Earthquake(); break;
                }

                m_NextAbilityTime = DateTime.Now + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            }
        }

        public void MoltenStrike(Mobile defender)
        {
            if (defender != null)
            {
                this.PlaySound(0x307);
                this.MovingEffect(defender, 0x36D4, 7, 0, false, false, 0x496, 0);
                AOS.Damage(defender, this, Utility.RandomMinMax(50, 80), 0, 100, 0, 0, 0);
                defender.SendLocalizedMessage(1070823); // The creature attacks with a fiery strike!
            }
        }

        public void ForgeArmor()
        {
            this.PlaySound(0x2A);
            this.FixedEffect(0x37C4, 10, 19, 1108, 4);
            this.VirtualArmor += 20;
            this.SendLocalizedMessage(1070845); // The creature fortifies its armor!
        }

        public void Earthquake()
        {
            ArrayList targets = new ArrayList();

            foreach (Mobile m in this.GetMobilesInRange(8))
            {
                if (m != this && m.Player && this.CanBeHarmful(m))
                    targets.Add(m);
            }

            this.PlaySound(0x2F3);

            for (int i = 0; i < targets.Count; ++i)
            {
                Mobile m = (Mobile)targets[i];

                double damage = m.Hits * 0.6;

                if (damage < 10.0)
                    damage = 10.0;
                else if (damage > 75.0)
                    damage = 75.0;

                DoHarmful(m);

                AOS.Damage(m, this, (int)damage, 100, 0, 0, 0, 0);

                if (m.Player)
                    m.SendLocalizedMessage(1153727); // The ground beneath your feet shakes and trembles.

                m.Freeze(TimeSpan.FromSeconds(3));
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