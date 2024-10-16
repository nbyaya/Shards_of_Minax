using System;
using System.Collections;
using Server.Engines.CannedEvil;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("corpse of Thomas Edison")]
    public class UltimateMasterTinkering : BaseChampion
    {
        private DateTime m_NextAbilityTime;

        [Constructable]
        public UltimateMasterTinkering()
            : base(AIType.AI_Melee)
        {
            Name = "Thomas Edison";
            Title = "The Inventor";
            Body = 0x190;
            Hue = 0x83F;

            SetStr(350, 475);
            SetDex(90, 150);
            SetInt(600, 800);

            SetHits(15000);
            SetMana(3000);

            SetDamage(30, 40);

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Energy, 50);

            SetResistance(ResistanceType.Physical, 70, 80);
            SetResistance(ResistanceType.Fire, 50, 60);
            SetResistance(ResistanceType.Cold, 50, 60);
            SetResistance(ResistanceType.Poison, 50, 60);
            SetResistance(ResistanceType.Energy, 80, 90);

            SetSkill(SkillName.Tinkering, 120.0);
            SetSkill(SkillName.MagicResist, 120.0);
            SetSkill(SkillName.Tactics, 120.0);
            SetSkill(SkillName.Wrestling, 120.0);

            Fame = 30000;
            Karma = -30000;

            VirtualArmor = 80;

            AddItem(new FancyShirt(Utility.RandomBlueHue()));
            AddItem(new LongPants(Utility.RandomNeutralHue()));
            AddItem(new Cloak(Utility.RandomYellowHue()));
            AddItem(new Sandals());

            HairItemID = 0x203B; // Short Hair
            HairHue = 0x47E;
        }

        public UltimateMasterTinkering(Serial serial)
            : base(serial)
        {
        }

        public override ChampionSkullType SkullType { get { return ChampionSkullType.Power; } }

        public override Type[] UniqueList
        {
            get { return new Type[] { typeof(InventorsToolkit), typeof(LightbulbPendant) }; }
        }

        public override Type[] SharedList
        {
            get { return new Type[] { typeof(TinkerTools), typeof(ClockworkAssembly) }; }
        }

        public override Type[] DecorativeList
        {
            get { return new Type[] { typeof(SmallClock), typeof(InventorsToolkit) }; }
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

            c.DropItem(new PowerScroll(SkillName.Tinkering, 200.0));

            if (Utility.RandomDouble() < 0.6)
                c.DropItem(new InventorsToolkit());

            if (Utility.RandomDouble() < 0.6)
                c.DropItem(new LightbulbPendant());
        }

        public override void OnGaveMeleeAttack(Mobile defender)
        {
            base.OnGaveMeleeAttack(defender);

            if (DateTime.Now > m_NextAbilityTime)
            {
                switch (Utility.Random(3))
                {
                    case 0: GadgetAttack(defender); break;
                    case 1: Repair(); break;
                    case 2: Electrify(defender); break;
                }

                m_NextAbilityTime = DateTime.Now + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            }
        }

        public void GadgetAttack(Mobile defender)
        {
            DoHarmful(defender);

            int damage = Utility.RandomMinMax(40, 60);

            AOS.Damage(defender, this, damage, 100, 0, 0, 0, 0);

            defender.FixedParticles(0x36BD, 20, 10, 5044, EffectLayer.Head);
            defender.PlaySound(0x207);
        }

        public void Repair()
        {
            ArrayList allies = new ArrayList();

            foreach (Mobile m in this.GetMobilesInRange(8))
            {
                if (m != this && m.Alive && !m.Player && this.CanBeBeneficial(m))
                    allies.Add(m);
            }

            for (int i = 0; i < allies.Count; ++i)
            {
                Mobile ally = (Mobile)allies[i];

                ally.Hits += Utility.RandomMinMax(30, 50);
                this.FixedParticles(0x376A, 9, 32, 5007, EffectLayer.Waist);
                this.PlaySound(0x1F2);
            }
        }

        public void Electrify(Mobile defender)
        {
            DoHarmful(defender);

            int damage = Utility.RandomMinMax(50, 70);

            AOS.Damage(defender, this, damage, 0, 0, 0, 0, 100);

            defender.FixedParticles(0x37CC, 20, 10, 5044, EffectLayer.Head);
            defender.Paralyze(TimeSpan.FromSeconds(3.0));
            defender.PlaySound(0x29);
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
