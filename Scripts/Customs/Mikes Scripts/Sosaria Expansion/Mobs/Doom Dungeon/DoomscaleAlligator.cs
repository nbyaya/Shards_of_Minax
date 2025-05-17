using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Spells;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a Doomscale Alligator corpse")]
    public class DoomscaleAlligator : BaseCreature
    {
        // Ability cooldown timers
        private DateTime m_NextAcidSpray, m_NextPoisonCloud, m_NextTailSweep, m_NextReflex;
        private Point3D  m_LastLocation;
        private int      m_LastDamageTaken;        // ◀️ track last damage taken

        private const int DoomHue = 1175;

        [Constructable]
        public DoomscaleAlligator()
            : base(AIType.AI_Melee, FightMode.Closest, 12, 1, 0.15, 0.3)
        {
            Name = "a Doomscale Alligator";
            Body = 0xCA;
            BaseSoundID = 660;
            Hue = DoomHue;

            // Stats
            SetStr(600, 700);
            SetDex(200, 250);
            SetInt(100, 150);

            SetHits(3000, 3500);
            SetStam(400, 450);
            SetMana(250, 300);

            SetDamage(25, 35);
            SetDamageType(ResistanceType.Physical, 70);
            SetDamageType(ResistanceType.Poison,   30);

            // Resistances
            SetResistance(ResistanceType.Physical, 60, 70);
            SetResistance(ResistanceType.Fire,     40, 50);
            SetResistance(ResistanceType.Cold,     40, 50);
            SetResistance(ResistanceType.Poison,   80, 90);
            SetResistance(ResistanceType.Energy,   50, 60);

            // Skills
            SetSkill(SkillName.MagicResist, 120.1, 140.0);
            SetSkill(SkillName.Tactics,     100.1, 110.0);
            SetSkill(SkillName.Wrestling,   100.1, 110.0);
            SetSkill(SkillName.Poisoning,   110.1, 120.0);

            Fame = 25000;
            Karma = -25000;
            VirtualArmor = 90;
            ControlSlots = 6;

            // Start cooldowns
            var now = DateTime.UtcNow;
            m_NextAcidSpray   = now + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 18));
            m_NextPoisonCloud = now + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            m_NextTailSweep   = now + TimeSpan.FromSeconds(Utility.RandomMinMax(8, 12));
            m_NextReflex      = now + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 22));

            m_LastLocation = this.Location;

            // Initial loot
            PackItem(new BlackPearl(Utility.RandomMinMax(15, 25)));
            PackItem(new SpidersSilk(Utility.RandomMinMax(10, 20)));
        }

        // Track incoming damage so we can reflect it
        public override void OnDamage(int amount, Mobile from, bool willKill)
        {
            base.OnDamage(amount, from, willKill);
            m_LastDamageTaken = amount;
        }

        // --- Aura: Slowing Acid Drip on movement ---
        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            base.OnMovement(m, oldLocation);

            if (m == null || m == this || !m.Alive || m.Map != this.Map)
                return;

            if (m.InRange(this.Location, 2) && CanBeHarmful(m, false))
            {
                DoHarmful(m);
                m.SendMessage("Acid drips onto you, slowing your steps!");

                // ◀️ Shoes → Feet
                m.FixedParticles(0x373A, 10, 15, 0xB73, EffectLayer.Head);



                // ◀️ swap StatMod ctor args: StatType first, then name
                int slowAmount = (int)(m.Dex * 0.2);
                var mod = new StatMod(StatType.Dex, "DoomAcidSlow", -slowAmount, TimeSpan.FromSeconds(5));
                m.AddStatMod(mod);
                Timer.DelayCall(TimeSpan.FromSeconds(5), () => m.RemoveStatMod("DoomAcidSlow"));
            }
        }

        public override void OnThink()
        {
            base.OnThink();

            if (!Alive || Map == null || Map == Map.Internal || Combatant == null)
                return;

            var now = DateTime.UtcNow;

            if (now >= m_NextTailSweep && InRange(Combatant.Location, 2))
            {
                TailSweepAttack();
                m_NextTailSweep = now + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 14));
            }
            else if (now >= m_NextAcidSpray && InRange(Combatant.Location, 8))
            {
                AcidSprayAttack();
                m_NextAcidSpray = now + TimeSpan.FromSeconds(Utility.RandomMinMax(14, 20));
            }
            else if (now >= m_NextPoisonCloud && InRange(Combatant.Location, 6))
            {
                PoisonCloudAttack();
                m_NextPoisonCloud = now + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 35));
            }
            else if (now >= m_NextReflex && InRange(Combatant.Location, 4))
            {
                DoomscaleReflex();
                m_NextReflex = now + TimeSpan.FromSeconds(Utility.RandomMinMax(18, 24));
            }

            // Leave corrosive ooze
            if (Location != m_LastLocation && Utility.RandomDouble() < 0.2)
            {
                var oozeLoc = m_LastLocation;
                m_LastLocation = Location;

                if (Map.CanFit(oozeLoc.X, oozeLoc.Y, oozeLoc.Z, 16, false, false))
                {
                    var tile = new QuicksandTile { Hue = DoomHue };
                    tile.MoveToWorld(oozeLoc, Map);
                }
            }
            else
            {
                m_LastLocation = Location;
            }
        }

        // --- Tail Sweep: knockback + bleeder ---
        public void TailSweepAttack()
        {
            Say("*RAAR!*");
            PlaySound(0x34B);

            var toPush = new List<Mobile>();
            foreach (var m in Map.GetMobilesInRange(Location, 2))
                if (m != this && CanBeHarmful(m, false))
                    toPush.Add(m);

            foreach (var target in toPush)
            {
                DoHarmful(target);

                AOS.Damage(target, this, Utility.RandomMinMax(30, 45), 100, 0, 0, 0, 0);

            }
        }

        // --- Acid Spray: ranged cone attack ---
        public void AcidSprayAttack()
        {
            if (!(Combatant is Mobile target) || !CanBeHarmful(target, false))
                return;

            Say("*GLAARRG!*");
            PlaySound(0x229);
            Effects.SendMovingParticles(
                new Entity(Serial.Zero, Location, Map),
                new Entity(Serial.Zero, target.Location, Map),
                0x36D4, 5, 0, false, true, DoomHue, 0, 9502, 1, 0, EffectLayer.Waist, 0x100
            );

            foreach (var m in Map.GetMobilesInRange(target.Location, 3))
            {
                if (m != this && CanBeHarmful(m, false))
                {
                    DoHarmful(m);
                    AOS.Damage(m, this, Utility.RandomMinMax(40, 60), 50, 0, 50, 0, 0);
                    m.ApplyPoison(this, Poison.Lethal);
                }
            }
        }

        // --- Poison Cloud: hazardous AoE tiles ---
        public void PoisonCloudAttack()
        {
            Say("*Sssss...*");
            PlaySound(0x292);

            for (int i = 0; i < 6; i++)
            {
                int x = X + Utility.RandomMinMax(-3, 3);
                int y = Y + Utility.RandomMinMax(-3, 3);
                int z = Z;
                if (!Map.CanFit(x, y, z, 16, false, false))
                    z = Map.GetAverageZ(x, y);

                var tile = new PoisonTile { Hue = DoomHue };
                tile.MoveToWorld(new Point3D(x, y, z), Map);
            }
        }

        // --- Doomscale Reflex: reflect & shred ---
        public void DoomscaleReflex()
        {
            Say("*Doom echoes in your strike!*");
            PlaySound(0x32A);

            if (Combatant is Mobile target && CanBeHarmful(target, false))
            {
                // reflect 25% of last hit
                int refl = (int)(m_LastDamageTaken * 0.25);
                if (refl > 0)
                    AOS.Damage(this, target, refl, 100, 0, 0, 0, 0);

                // shred physical resistance by 10% for 10s
                target.SendMessage("Your defenses are corroded!");
                target.FixedParticles(0x3664, 10, 30, 0xB73, EffectLayer.Waist);

                var mod = new ResistanceMod(ResistanceType.Physical, -10);
                target.AddResistanceMod(mod);
                Timer.DelayCall(TimeSpan.FromSeconds(10), () => target.RemoveResistanceMod(mod));
            }
        }

        public override void OnDeath(Container c)
        {
            
			base.OnDeath(c);
			
			if (Map == null) return;

            Say("*...the swamp reclaims its own...*");
            Effects.PlaySound(Location, Map, 0x658);

            for (int i = 0; i < 8; i++)
            {
                int x = X + Utility.RandomMinMax(-4, 4);
                int y = Y + Utility.RandomMinMax(-4, 4);
                int z = Z;
                if (!Map.CanFit(x, y, z, 16, false, false))
                    z = Map.GetAverageZ(x, y);

                var tile = new QuicksandTile { Hue = DoomHue };
                tile.MoveToWorld(new Point3D(x, y, z), Map);
            }
        }

        // Standard properties & loot
        public override bool BleedImmune => true;
        public override Poison PoisonImmune => Poison.Lethal;
        public override int TreasureMapLevel => 6;
        public override double DispelDifficulty => 150.0;
        public override double DispelFocus     => 80.0;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 2);
            AddLoot(LootPack.Gems, Utility.RandomMinMax(10, 15));
            if (Utility.RandomDouble() < 0.03)
                PackItem(new PowerCrystal());
        }

        public DoomscaleAlligator(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            reader.ReadInt();

            // reset timers
            var now = DateTime.UtcNow;
            m_NextAcidSpray   = now + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 18));
            m_NextPoisonCloud = now + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            m_NextTailSweep   = now + TimeSpan.FromSeconds(Utility.RandomMinMax(8, 12));
            m_NextReflex      = now + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 22));
        }
    }
}
