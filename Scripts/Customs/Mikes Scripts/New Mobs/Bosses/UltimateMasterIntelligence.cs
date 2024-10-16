using System;
using System.Collections;
using System.Collections.Generic;
using Server.Engines.CannedEvil;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("corpse of Albert Einstein")]
    public class UltimateMasterIntelligence : BaseChampion
    {
        private DateTime m_NextAbilityTime;
        private Dictionary<string, bool> m_Actions;

        [Constructable]
        public UltimateMasterIntelligence()
            : base(AIType.AI_Mage)
        {
            Name = "Albert Einstein";
            Title = "The Ultimate Master of Intelligence";
            Body = 0x190;
            Hue = 0x83F;

            SetStr(305, 425);
            SetDex(72, 150);
            SetInt(705, 950);

            SetHits(15000);
            SetMana(4000);

            SetDamage(25, 35);

            SetDamageType(ResistanceType.Physical, 25);
            SetDamageType(ResistanceType.Energy, 75);

            SetResistance(ResistanceType.Physical, 60, 70);
            SetResistance(ResistanceType.Fire, 60, 70);
            SetResistance(ResistanceType.Cold, 60, 70);
            SetResistance(ResistanceType.Poison, 60, 70);
            SetResistance(ResistanceType.Energy, 90, 100);

            SetSkill(SkillName.EvalInt, 120.0);
            SetSkill(SkillName.Magery, 120.0);
            SetSkill(SkillName.Meditation, 120.0);
            SetSkill(SkillName.MagicResist, 120.0);
            SetSkill(SkillName.Tactics, 120.0);
            SetSkill(SkillName.Wrestling, 120.0);

            Fame = 25000;
            Karma = 25000;

            VirtualArmor = 70;
			
            AddItem(new FancyShirt(0x0));
            AddItem(new LongPants(0x0));
            AddItem(new Sandals());

            HairItemID = 0x203C; // Wild Hair
            HairHue = 0x386; // Gray
            FacialHairItemID = 0x203E; // Medium Short Beard
            FacialHairHue = 0x386; // Gray

            m_Actions = new Dictionary<string, bool>();
        }

        public UltimateMasterIntelligence(Serial serial)
            : base(serial)
        {
        }

        public override ChampionSkullType SkullType { get { return ChampionSkullType.Power; } }

        public override Type[] UniqueList
        {
            get { return new Type[] { typeof(RelativityManual), typeof(GeniusGlasses) }; }
        }

        public override Type[] SharedList
        {
            get { return new Type[] { typeof(EinsteinChalkboard), typeof(TheoryOfRelativityScroll) }; }
        }

        public override Type[] DecorativeList
        {
            get { return new Type[] { typeof(RelativityManual), typeof(EinsteinPipe) }; }
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

            c.DropItem(new PowerScroll(SkillName.EvalInt, 200.0));

            if (Utility.RandomDouble() < 0.6)
                c.DropItem(new RelativityManual());

            if (Utility.RandomDouble() < 0.6)
                c.DropItem(new GeniusGlasses());
        }

        public override void OnGaveMeleeAttack(Mobile defender)
        {
            base.OnGaveMeleeAttack(defender);

            if (DateTime.Now > m_NextAbilityTime)
            {
                switch (Utility.Random(3))
                {
                    case 0: EnergyBlast(); break;
                    case 1: ThoughtAcceleration(); break;
                    case 2: MindOverMatter(); break;
                }

                m_NextAbilityTime = DateTime.Now + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            }
        }

        public void EnergyBlast()
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

                AOS.Damage(m, this, damage, 0, 0, 0, 0, 100);

                m.FixedParticles(0x3818, 1, 11, 0x13A8, 0, 0, EffectLayer.Waist);
                m.PlaySound(0x201);
            }
        }

        public void ThoughtAcceleration()
        {
            FixedParticles(0x376A, 9, 32, 5030, EffectLayer.Waist);
            PlaySound(0x1E9);

            BeginAction("ThoughtAcceleration");
            Timer.DelayCall(TimeSpan.FromSeconds(10.0), new TimerCallback(EndThoughtAcceleration));
        }

        public void EndThoughtAcceleration()
        {
            EndAction("ThoughtAcceleration");
        }

        public void MindOverMatter()
        {
            FixedParticles(0x375A, 9, 20, 5027, EffectLayer.Waist);
            PlaySound(0x1ED);

            BeginAction("MindOverMatter");
            Timer.DelayCall(TimeSpan.FromSeconds(15.0), new TimerCallback(EndMindOverMatter));
        }

        public void EndMindOverMatter()
        {
            EndAction("MindOverMatter");
        }

        public override int GetResistance(ResistanceType type)
        {
            int resistance = base.GetResistance(type);

            if (HasAction("MindOverMatter"))
            {
                resistance += 25;
            }

            return resistance;
        }

        public bool HasAction(string action)
        {
            return m_Actions.ContainsKey(action) && m_Actions[action];
        }

        public void BeginAction(string action)
        {
            m_Actions[action] = true;
        }

        public void EndAction(string action)
        {
            if (m_Actions.ContainsKey(action))
                m_Actions[action] = false;
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

            m_Actions = new Dictionary<string, bool>();
        }
    }
}