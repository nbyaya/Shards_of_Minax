using System;
using System.Collections;
using Server.Engines.CannedEvil;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("corpse of Pan")]
    public class UltimateMasterProvocation : BaseChampion
    {
        private DateTime m_NextAbilityTime;

        [Constructable]
        public UltimateMasterProvocation()
            : base(AIType.AI_Mage)
        {
            Name = "Pan";
            Title = "The Greek God of the Wild";
            Body = 0x190;
            Hue = 0x83F;

            SetStr(325, 450);
            SetDex(100, 150);
            SetInt(600, 800);

            SetHits(14000);
            SetMana(3000);

            SetDamage(30, 40);

            SetDamageType(ResistanceType.Physical, 25);
            SetDamageType(ResistanceType.Cold, 25);
            SetDamageType(ResistanceType.Poison, 25);
            SetDamageType(ResistanceType.Energy, 25);

            SetResistance(ResistanceType.Physical, 60, 70);
            SetResistance(ResistanceType.Fire, 50, 60);
            SetResistance(ResistanceType.Cold, 70, 80);
            SetResistance(ResistanceType.Poison, 80, 90);
            SetResistance(ResistanceType.Energy, 60, 70);

            SetSkill(SkillName.EvalInt, 120.0);
            SetSkill(SkillName.Magery, 120.0);
            SetSkill(SkillName.Meditation, 120.0);
            SetSkill(SkillName.MagicResist, 120.0);
            SetSkill(SkillName.Tactics, 120.0);
            SetSkill(SkillName.Wrestling, 120.0);
            SetSkill(SkillName.Provocation, 120.0);

            Fame = 24000;
            Karma = -24000;

            VirtualArmor = 75;

            AddItem(new FancyShirt(Utility.RandomRedHue()));
            AddItem(new LongPants(Utility.RandomGreenHue()));
            AddItem(new Cloak(Utility.RandomGreenHue()));
            AddItem(new Sandals());

            HairItemID = 0x203B; // Short Hair
            HairHue = 0x94;
        }

        public UltimateMasterProvocation(Serial serial)
            : base(serial)
        {
        }

        public override ChampionSkullType SkullType { get { return ChampionSkullType.Power; } }

        public override Type[] UniqueList
        {
            get { return new Type[] { typeof(PipesOfPan), typeof(WildCloak) }; }
        }

        public override Type[] SharedList
        {
            get { return new Type[] { typeof(MusicalSheet), typeof(Harp) }; }
        }

        public override Type[] DecorativeList
        {
            get { return new Type[] { typeof(MusicalSheet), typeof(WoodlandStatue) }; }
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

            c.DropItem(new PowerScroll(SkillName.Provocation, 200.0));

            if (Utility.RandomDouble() < 0.6)
                c.DropItem(new PipesOfPan());

            if (Utility.RandomDouble() < 0.6)
                c.DropItem(new WildCloak());
        }

        public override void OnGaveMeleeAttack(Mobile defender)
        {
            base.OnGaveMeleeAttack(defender);

            if (DateTime.Now > m_NextAbilityTime)
            {
                switch (Utility.Random(3))
                {
                    case 0: RaucousMelody(); break;
                    case 1: ForestGuardians(); break;
                    case 2: DiscordantNote(); break;
                }

                m_NextAbilityTime = DateTime.Now + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            }
        }

        public void RaucousMelody()
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

                m.FixedParticles(0x376A, 9, 32, 5008, EffectLayer.Waist);
                m.PlaySound(0x1FA);
            }
        }

        public void ForestGuardians()
        {
            // Summoning woodland creatures logic
            for (int i = 0; i < Utility.RandomMinMax(3, 5); i++)
            {
                BaseCreature creature = new DireWolf();
                creature.MoveToWorld(this.Location, this.Map);
                creature.Combatant = this.Combatant;
            }
        }

        public void DiscordantNote()
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

                // Logic for debuffing enemies' defenses
                m.SendMessage("You are struck by a discordant note!");
                m.PlaySound(0x1F5);
                m.FixedParticles(0x376A, 9, 32, 5008, EffectLayer.Waist);

                // Apply debuff
                m.AddStatMod(new StatMod(StatType.Dex, "DiscordantNoteDex", -20, TimeSpan.FromSeconds(10.0)));
                m.AddStatMod(new StatMod(StatType.Int, "DiscordantNoteInt", -20, TimeSpan.FromSeconds(10.0)));
                m.AddStatMod(new StatMod(StatType.Str, "DiscordantNoteStr", -20, TimeSpan.FromSeconds(10.0)));
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
