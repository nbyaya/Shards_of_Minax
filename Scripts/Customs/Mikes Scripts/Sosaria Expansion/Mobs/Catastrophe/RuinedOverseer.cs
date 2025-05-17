using System;
using Server;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a ruined overseer's corpse")]
    public class RuinedOverseer : BaseCreature
    {
        private bool m_FieldActive;
        private DateTime _nextRuinBlast;
        private DateTime _nextSoulSiphon;

        [Constructable]
        public RuinedOverseer()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            this.Name = "ruined overseer";
            this.Body = 0x2F4;
            this.Hue = 0xB81; // unique hue for the Ruined Overseer
            this.BaseSoundID = 0x23B;

            // Advanced stats – noticeably more powerful than the original
            this.SetStr(701, 800);
            this.SetDex(100, 120);
            this.SetInt(90, 120);

            this.SetHits(500, 600);

            // Damage: a mix of physical and energy damage
            this.SetDamage(20, 30);
            this.SetDamageType(ResistanceType.Physical, 50);
            this.SetDamageType(ResistanceType.Energy, 50);

            // Increased resistances
            this.SetResistance(ResistanceType.Physical, 50, 70);
            this.SetResistance(ResistanceType.Fire, 45, 60);
            this.SetResistance(ResistanceType.Cold, 40, 55);
            this.SetResistance(ResistanceType.Poison, 50, 60);
            this.SetResistance(ResistanceType.Energy, 50, 65);

            // Skills boosted for advanced combat
            this.SetSkill(SkillName.MagicResist, 90.0, 110.0);
            this.SetSkill(SkillName.Tactics, 90.0, 110.0);
            this.SetSkill(SkillName.Wrestling, 90.0, 110.0);

            this.Fame = 20000;
            this.Karma = -20000;
            this.VirtualArmor = 75;



            // Initialize the magical field status
            m_FieldActive = CanUseField;

            // Setup timers for advanced abilities
            _nextRuinBlast = DateTime.UtcNow + TimeSpan.FromSeconds(10.0);
            _nextSoulSiphon = DateTime.UtcNow + TimeSpan.FromSeconds(5.0);
        }

        public RuinedOverseer(Serial serial)
            : base(serial)
        {
        }

        // The Ruined Overseer’s field is active only while nearly uninjured
        public bool FieldActive { get { return m_FieldActive; } }
        public bool CanUseField { get { return this.Hits >= this.HitsMax * 9 / 10; } }

        public override bool IsScaredOfScaryThings { get { return false; } }
        public override bool IsScaryToPets { get { return true; } }
        public override bool BardImmune { get { return !Core.AOS; } }
        public override Poison PoisonImmune { get { return Poison.Lethal; } }

        public override void GenerateLoot()
        {
            this.AddLoot(LootPack.UltraRich);
            this.AddLoot(LootPack.Potions);
        }

        public override int GetIdleSound() { return 0xFD; }
        public override int GetAngerSound() { return 0x26C; }
        public override int GetDeathSound() { return 0x211; }
        public override int GetAttackSound() { return 0x23B; }
        public override int GetHurtSound() { return 0x140; }

        // When damaged by a spell, the Ruined Overseer may counterattack with a Ruin Bolt.
        public override void OnDamagedBySpell(Mobile from)
        {
            if (from == null || from.Deleted)
                return;

            if (from is Mobile target) // ensure target-specific properties are safe to access
            {
                if (0.4 > Utility.RandomDouble())
                    SendRuinBolt(target);
            }

            if (!m_FieldActive)
            {
                // Absorb the spell and trigger an area explosion effect when its field is down.
                this.FixedParticles(0, 10, 0, 0x2522, EffectLayer.Waist);
                ExplodeRuinField();
            }
            else if (m_FieldActive && !CanUseField)
            {
                m_FieldActive = false;
                this.FixedParticles(0x3735, 1, 30, 0x251F, EffectLayer.Waist);
            }
        }

        // On receiving a melee attack, the monster will flash its barrier and attempt a Soul Siphon.
        public override void OnGotMeleeAttack(Mobile attacker)
        {
            base.OnGotMeleeAttack(attacker);

            if (m_FieldActive)
            {
                this.FixedParticles(0x376A, 20, 10, 0x2530, EffectLayer.Waist);
                this.PlaySound(0x2F4);
                attacker.SendAsciiMessage("Your weapon is repelled by the Overseer's barrier!");
            }

            // Chance (25%) to drain life from the attacker if the cooldown has expired.
            if (attacker != null && attacker.Alive && DateTime.UtcNow >= _nextSoulSiphon)
            {
                if (Utility.RandomDouble() < 0.25 && attacker is Mobile target)
                {
                    SoulSiphon(target);
                    _nextSoulSiphon = DateTime.UtcNow + TimeSpan.FromSeconds(7.0);
                }
            }
        }

        // When the Ruined Overseer strikes in melee, it may inflict a devastating Ruin Curse.
        public override void OnGaveMeleeAttack(Mobile defender)
        {
            base.OnGaveMeleeAttack(defender);

            if (defender != null && defender.Alive && defender is Mobile target)
            {
                if (Utility.RandomDouble() < 0.20)
                {
                    // The Ruin Curse stuns and damages the target.
                    target.SendLocalizedMessage(1042001); // e.g., "You feel your essence wither!"
                    target.Freeze(TimeSpan.FromSeconds(3.0));
                    AOS.Damage(target, this, Utility.RandomMinMax(10, 20), 50, 0, 0, 50, 0);
                }
            }
        }

        // OnActionCombat periodically triggers the Ruin Blast ability.
        public override void OnActionCombat()
        {
            Mobile combatant = Combatant as Mobile;
            if (combatant == null || combatant.Deleted || combatant.Map != this.Map || !InRange(combatant, 12) || !CanBeHarmful(combatant) || !InLOS(combatant))
                return;

            if (DateTime.UtcNow >= _nextRuinBlast)
            {
                EmitRuinBlast(combatant);
                _nextRuinBlast = DateTime.UtcNow + TimeSpan.FromSeconds(15.0 + (10.0 * Utility.RandomDouble()));
            }
        }

        // Ruin Blast: an area attack that damages all nearby foes.
        public void EmitRuinBlast(Mobile target)
        {
            // Show a burst of spectral energy.
            this.FixedParticles(0x376A, 30, 15, 0x2530, EffectLayer.Waist);
            this.PlaySound(0x211);

            foreach (Mobile m in this.GetMobilesInRange(3))
            {
                if (m != this && CanBeHarmful(m))
                {
                    DoHarmful(m);
                    int damage = Utility.RandomMinMax(25, 40);
                    // Damage is delivered as a mix of physical and energy.
                    AOS.Damage(m, this, damage, 50, 0, 0, 50, 0);
                }
            }
        }

        // Soul Siphon: the Ruined Overseer drains life from its target, healing itself.
        public void SoulSiphon(Mobile target)
        {
            if (target == null || target.Deleted)
                return;

            DoHarmful(target);
            int damage = Utility.RandomMinMax(15, 25);
            AOS.Damage(target, this, damage, 100, 0, 0, 0, 0);
            this.Hits += damage / 2; // Heal itself by half the damage dealt

            // Show draining particles using the unique hue.
            this.MovingParticles(target, 0x379F, 7, 0, false, true, 0xB81, 0xFCB, 0x211);
            if (this.Hits > this.HitsMax)
                this.Hits = this.HitsMax;
        }

        // Ruin Bolt: fires a powerful energy bolt at a target.
        public void SendRuinBolt(Mobile target)
        {
            if (target == null || target.Deleted)
                return;

            this.MovingParticles(target, 0x379F, 10, 0, false, true, 0xB81, 0xFCB, 0x211);
            target.PlaySound(0x229);
            DoHarmful(target);
            AOS.Damage(target, this, 50, 0, 0, 0, 0, 100);
        }

        // Explode Ruin Field: when the magical field is down, it causes a burst that damages all nearby enemies.
        public void ExplodeRuinField()
        {
            foreach (Mobile m in this.GetMobilesInRange(2))
            {
                if (m != this && CanBeHarmful(m))
                {
                    DoHarmful(m);
                    int explosionDamage = Utility.RandomMinMax(20, 30);
                    AOS.Damage(m, this, explosionDamage, 50, 0, 0, 50, 0);
                }
            }
        }

        // Periodically regenerate the Ruin Field if the Overseer is not hurt.
        public override void OnThink()
        {
            base.OnThink();

            if (!m_FieldActive && !IsHurt())
                m_FieldActive = true;
        }

        public override bool Move(Direction d)
        {
            bool move = base.Move(d);

            if (move && m_FieldActive && Combatant != null && Combatant is Mobile)
                this.FixedParticles(0, 10, 0, 0x2530, EffectLayer.Waist);

            return move;
        }

        // When the Ruined Overseer is protected by its field, no melee damage penetrates.
        public override void AlterMeleeDamageFrom(Mobile from, ref int damage)
        {
            if (m_FieldActive)
                damage = 0;
        }

        // When its field is down, spells are negated.
        public override void AlterSpellDamageFrom(Mobile caster, ref int damage)
        {
            if (!m_FieldActive)
                damage = 0;
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

            m_FieldActive = CanUseField;
        }
    }
}
