using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a shattered cosplayer's costume")]
    public class Cosplayer : BaseCreature
    {
        // Timers for special abilities
        private DateTime m_NextDisorientTime;
        private DateTime m_NextShatterTime;
        private DateTime m_NextShiftTime;

        // Initial unique hue
        private const int UniqueHue = 1175;

        // Possible hues when shifting costumes
        private static readonly int[] ShiftHues = new int[] { 1157, 1175, 1256, 1337 };

        [Constructable]
        public Cosplayer()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a cosplayer";
            Body = 0x190;                   // YoungRonin body
            BaseSoundID = 0x3A;             // Human melee sounds
            Hue = UniqueHue;

            // — Stats —
            SetStr(350, 400);
            SetDex(200, 250);
            SetInt(450, 500);

            SetHits(1200, 1400);
            SetStam(250, 300);
            SetMana(700, 800);

            // — Damage —
            SetDamage(20, 30);
            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Energy, 50);

            // — Resistances —
            SetResistance(ResistanceType.Physical, 50, 60);
            SetResistance(ResistanceType.Fire,     40, 50);
            SetResistance(ResistanceType.Cold,     40, 50);
            SetResistance(ResistanceType.Poison,   60, 70);
            SetResistance(ResistanceType.Energy,   80, 90);

            // — Skills —
            SetSkill(SkillName.Swords,       110.0, 120.0);
            SetSkill(SkillName.Tactics,      110.0, 120.0);
            SetSkill(SkillName.Magery,       100.0, 110.0);
            SetSkill(SkillName.MagicResist,  100.0, 110.0);
            SetSkill(SkillName.Wrestling,     90.0, 100.0);

            Utility.AssignRandomHair(this);
            Utility.AssignRandomFacialHair(this);

            this.AddItem(new LeatherDo());
            this.AddItem(new LeatherHiroSode());
            this.AddItem(new SamuraiTabi());

            switch ( Utility.Random(3) )
            {
                case 0:
                    this.AddItem(new StuddedHaidate());
                    break;
                case 1:
                    this.AddItem(new PlateSuneate());
                    break;
                default:
                    this.AddItem(new LeatherSuneate());
                    break;
            }

            this.AddItem(new Bandana(Utility.RandomNondyedHue()));

            switch ( Utility.Random(3) )
            {
                case 0:
                    this.AddItem(new NoDachi());
                    break;
                case 1:
                    this.AddItem(new Lajatang());
                    break;
                default:
                    this.AddItem(new Wakizashi());
                    break;
            }

            Fame = 18000;
            Karma = -18000;

            VirtualArmor = 90;
            ControlSlots = 4;

            // Initialize cooldowns
            m_NextDisorientTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_NextShatterTime    = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 25));
            m_NextShiftTime      = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(30, 40));

            // Base loot
            PackItem(new Cloth(Utility.RandomMinMax(15, 25)));
            PackItem(new Leather(Utility.RandomMinMax(10, 20)));
        }

        // Aura on movement: small damage + stamina drain
        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            if (m != null && m != this && m.Map == this.Map && m.InRange(this, 2) && Alive && m.Alive && CanBeHarmful(m, false))
            {
                if (m is Mobile targetMobile)
                {
                    DoHarmful(targetMobile);

                    // Damage + drain
                    int dmg = Utility.RandomMinMax(5, 10);
                    AOS.Damage(targetMobile, this, dmg, 0, 0, 0, 0, 100);

                    // Stam drain
                    int drain = Utility.RandomMinMax(10, 20);
                    if (targetMobile.Stam >= drain)
                        targetMobile.Stam -= drain;

                    targetMobile.SendMessage("The cosplayer's aura saps your energy!");
                    targetMobile.FixedParticles(0x374A, 10, 15, 5032, EffectLayer.Head);
                    targetMobile.PlaySound(0x1F8);
                }
            }

            base.OnMovement(m, oldLocation);
        }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant == null || !Alive || Map == null || Map == Map.Internal)
                return;

            // Costume Shift
            if (DateTime.UtcNow >= m_NextShiftTime)
            {
                CostumeShift();
                m_NextShiftTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(30, 40));
            }
            // Shard Barrage
            else if (DateTime.UtcNow >= m_NextShatterTime && InRange(Combatant.Location, 12))
            {
                ShardBarrage();
                m_NextShatterTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 25));
            }
            // Disorient Strike
            else if (DateTime.UtcNow >= m_NextDisorientTime && InRange(Combatant.Location, 8))
            {
                DisorientStrike();
                m_NextDisorientTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            }
        }

        // — Ability 1: Costume Shift — random color + self‑heal
        public void CostumeShift()
        {
            this.Say("Look at my new outfit!");
            PlaySound(0x3D);

            // Pick a new hue and apply
            Hue = ShiftHues[Utility.Random(ShiftHues.Length)];

            // Heal a bit of health & mana
            int heal = Utility.RandomMinMax(50, 100);
            Hits += heal;
            Mana += heal;

            FixedParticles(0x376A, 10, 15, 5032, EffectLayer.CenterFeet);
        }

        // — Ability 2: Disorient Strike — paralyze + heavy damage
        public void DisorientStrike()
        {
            if (Combatant is Mobile target)
            {
                this.Say("*Confusion!*");
                PlaySound(0x2F);

                target.Paralyze(TimeSpan.FromSeconds(2));
                FixedParticles(0x376A, 10, 20, 5032, EffectLayer.Head);

                DoHarmful(target);
                AOS.Damage(target, this, Utility.RandomMinMax(40, 60), 0, 0, 100, 0, 0);
            }
        }

        // — Ability 3: Shard Barrage — spawns IceShardTile hazards around the target
        public void ShardBarrage()
        {
            if (Combatant is Mobile target)
            {
                this.Say("*Feel the cosplay shards!*");
                PlaySound(0x64);

                Point3D loc = target.Location;
                for (int i = 0; i < 6; i++)
                {
                    int x = loc.X + Utility.RandomMinMax(-2, 2);
                    int y = loc.Y + Utility.RandomMinMax(-2, 2);
                    int z = loc.Z;
                    if (!Map.CanFit(x, y, z, 16, false, false))
                        z = Map.GetAverageZ(x, y);

                    var tile = new IceShardTile();
                    tile.Hue = Hue;
                    tile.MoveToWorld(new Point3D(x, y, z), Map);
                }
            }
        }

        // — Death: spawn webs & poison clouds —
        public override void OnDeath(Container c)
        {
            if (Map != null)
            {
                this.Say("My costume... shattered!");
                PlaySound(0x229);

                for (int i = 0; i < 4; i++)
                {
                    int x = X + Utility.RandomMinMax(-3, 3);
                    int y = Y + Utility.RandomMinMax(-3, 3);
                    int z = Z;
                    if (!Map.CanFit(x, y, z, 16, false, false))
                        z = Map.GetAverageZ(x, y);

                    // Trap Web
                    var web = new TrapWeb();
                    web.MoveToWorld(new Point3D(x, y, z), Map);

                    // Poison Cloud
                    var poison = new PoisonTile();
                    poison.Hue = Hue;
                    poison.MoveToWorld(new Point3D(x, y, z), Map);
                }
            }

            base.OnDeath(c);
        }

        // — Loot & Properties —
        public override bool BleedImmune { get { return true; } }
        public override Poison PoisonImmune { get { return Poison.Lethal; } }
        public override int TreasureMapLevel { get { return 6; } }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.FilthyRich);
            AddLoot(LootPack.Rich);

            // 5% chance to drop a unique cosplay mask
            if (Utility.RandomDouble() < 0.05)
                PackItem(new IronOrchardPauldrons());
        }

        public Cosplayer(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            var version = reader.ReadInt();
        }
    }
}
