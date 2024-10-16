using System;
using System.Collections;
using Server.Engines.CannedEvil;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("corpse of Quincy M.E.")]
    public class UltimateMasterForensicEvaluator : BaseChampion
    {
        private DateTime m_NextAbilityTime;

        [Constructable]
        public UltimateMasterForensicEvaluator()
            : base(AIType.AI_Mage)
        {
            Name = "Quincy M.E.";
            Title = "Master Medical Examiner";
            Body = 0x190;
            Hue = 0x83E;

            SetStr(275, 400);
            SetDex(70, 150);
            SetInt(520, 750);

            SetHits(11000);
            SetMana(2600);

            SetDamage(20, 30);

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Energy, 50);

            SetResistance(ResistanceType.Physical, 55, 65);
            SetResistance(ResistanceType.Fire, 50, 60);
            SetResistance(ResistanceType.Cold, 50, 60);
            SetResistance(ResistanceType.Poison, 80, 90);
            SetResistance(ResistanceType.Energy, 60, 70);

            SetSkill(SkillName.EvalInt, 120.0);
            SetSkill(SkillName.Magery, 120.0);
            SetSkill(SkillName.Meditation, 120.0);
            SetSkill(SkillName.Forensics, 120.0);
            SetSkill(SkillName.MagicResist, 120.0);
            SetSkill(SkillName.Tactics, 120.0);
            SetSkill(SkillName.Wrestling, 120.0);

            Fame = 24000;
            Karma = -24000;

            VirtualArmor = 60;
			
			AddItem(new Robe(Utility.RandomBlueHue()));
            AddItem(new Boots());
            AddItem(new WizardsHat());

            HairItemID = 0x2048; // Short Hair
            HairHue = 0x47E;
        }

        public UltimateMasterForensicEvaluator(Serial serial)
            : base(serial)
        {
        }

        public override ChampionSkullType SkullType { get { return ChampionSkullType.Power; } }

        public override Type[] UniqueList
        {
            get { return new Type[] { typeof(ForensicKit), typeof(MedicalExaminersBadge) }; }
        }

        public override Type[] SharedList
        {
            get { return new Type[] { typeof(InvestigatorNotes), typeof(MagnifyingGlass) }; }
        }

        public override Type[] DecorativeList
        {
            get { return new Type[] { typeof(ForensicKit), typeof(CrimeSceneTape) }; }
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

            c.DropItem(new PowerScroll(SkillName.Forensics, 200.0));

            if (Utility.RandomDouble() < 0.6)
                c.DropItem(new InvestigatorNotes());

            if (Utility.RandomDouble() < 0.6)
                c.DropItem(new MagnifyingGlass());
        }

        public override void OnGaveMeleeAttack(Mobile defender)
        {
            base.OnGaveMeleeAttack(defender);

            if (DateTime.Now > m_NextAbilityTime)
            {
                switch (Utility.Random(3))
                {
                    case 0: Autopsy(); break;
                    case 1: EvidenceCollection(); break;
                    case 2: InvestigativeStrike(defender); break;
                }

                m_NextAbilityTime = DateTime.Now + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            }
        }

        public void Autopsy()
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

                int damage = Utility.RandomMinMax(50, 70);

                AOS.Damage(m, this, damage, 100, 0, 0, 0, 0);

                m.FixedParticles(0x374A, 10, 15, 5038, EffectLayer.Head);
                m.PlaySound(0x1F1);
            }
        }

        public void EvidenceCollection()
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

                m.SendMessage("You feel weaker as your strength is drained!");

                m.AddStatMod(new StatMod(StatType.Str, "ForensicEval-StrengthDrain", -10, TimeSpan.FromSeconds(30)));

                this.Hits += 10;
            }
        }

        public void InvestigativeStrike(Mobile defender)
        {
            if (defender != null)
            {
                defender.FixedParticles(0x374A, 10, 15, 5038, EffectLayer.Head);
                defender.PlaySound(0x1F1);

                defender.AddStatMod(new StatMod(StatType.Dex, "ForensicEval-DexDebuff", -10, TimeSpan.FromSeconds(30)));
                defender.AddStatMod(new StatMod(StatType.Int, "ForensicEval-IntDebuff", -10, TimeSpan.FromSeconds(30)));
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
