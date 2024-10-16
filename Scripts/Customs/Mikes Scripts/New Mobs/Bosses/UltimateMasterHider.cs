using System;
using System.Collections;
using Server.Engines.CannedEvil;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("corpse of Harry Houdini")]
    public class UltimateMasterHider : BaseChampion
    {
        private DateTime m_NextAbilityTime;

        [Constructable]
        public UltimateMasterHider()
            : base(AIType.AI_Melee)
        {
            Name = "Harry Houdini";
            Title = "The Ultimate Escape Artist";
            Body = 0x190;
            Hue = 0x83EA;

            SetStr(305, 425);
            SetDex(72, 150);
            SetInt(505, 750);

            SetHits(12000);
            SetMana(2500);

            SetDamage(25, 35);

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Cold, 25);
            SetDamageType(ResistanceType.Poison, 25);

            SetResistance(ResistanceType.Physical, 60, 70);
            SetResistance(ResistanceType.Fire, 50, 60);
            SetResistance(ResistanceType.Cold, 60, 70);
            SetResistance(ResistanceType.Poison, 60, 70);
            SetResistance(ResistanceType.Energy, 50, 60);

            SetSkill(SkillName.Hiding, 120.0);
            SetSkill(SkillName.Stealth, 120.0);
            SetSkill(SkillName.Lockpicking, 120.0);
            SetSkill(SkillName.EvalInt, 120.0);
            SetSkill(SkillName.MagicResist, 120.0);
            SetSkill(SkillName.Tactics, 120.0);
            SetSkill(SkillName.Wrestling, 120.0);

            Fame = 22500;
            Karma = -22500;

            VirtualArmor = 70;

            AddItem(new FancyShirt(Utility.RandomRedHue()));
            AddItem(new LongPants(Utility.RandomYellowHue()));
            AddItem(new Cloak(Utility.RandomBlueHue()));
            AddItem(new Sandals());

            HairItemID = 0x203B; // Short Hair
            HairHue = 0x47E;
        }

        public UltimateMasterHider(Serial serial)
            : base(serial)
        {
        }

        public override ChampionSkullType SkullType { get { return ChampionSkullType.Power; } }

        public override Type[] UniqueList
        {
            get { return new Type[] { typeof(HoudiniCloak), typeof(LockpickSet) }; }
        }

        public override Type[] SharedList
        {
            get { return new Type[] { typeof(HoudiniTome), typeof(MasterLockpicks) }; }
        }

        public override Type[] DecorativeList
        {
            get { return new Type[] { typeof(HoudiniCloak), typeof(MagiciansHat) }; }
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

            c.DropItem(new PowerScroll(SkillName.Hiding, 200.0));

            if (Utility.RandomDouble() < 0.6)
                c.DropItem(new HoudiniCloak());

            if (Utility.RandomDouble() < 0.6)
                c.DropItem(new LockpickSet());
        }

        public override void OnGaveMeleeAttack(Mobile defender)
        {
            base.OnGaveMeleeAttack(defender);

            if (DateTime.Now > m_NextAbilityTime)
            {
                switch (Utility.Random(3))
                {
                    case 0: VanishingAct(); break;
                    case 1: EscapeArtist(); break;
                    case 2: Illusion(); break;
                }

                m_NextAbilityTime = DateTime.Now + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            }
        }

        public void VanishingAct()
        {
            Effects.SendLocationParticles(EffectItem.Create(Location, Map, EffectItem.DefaultDuration), 0x376A, 9, 32, 5022);
            PlaySound(0x1FD);
            Hidden = true;
        }

        public void EscapeArtist()
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

                if (m is PlayerMobile)
                {
                    ((PlayerMobile)m).Frozen = false;
                    ((PlayerMobile)m).Paralyzed = false;
                    m.SendLocalizedMessage(1075857); // You have been freed from paralysis.
                }
            }
        }

        public void Illusion()
        {
            Mobile decoy = new BaseCreature(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4);
            decoy.MoveToWorld(Location, Map);
            decoy.Hue = Hue;
            decoy.Body = Body;
            decoy.Name = "Decoy of " + Name;

            Effects.SendLocationParticles(EffectItem.Create(decoy.Location, decoy.Map, EffectItem.DefaultDuration), 0x376A, 9, 32, 5022);
            decoy.PlaySound(0x1FD);
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
